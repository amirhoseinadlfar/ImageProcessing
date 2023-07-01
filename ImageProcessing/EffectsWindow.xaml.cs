using ImageProcessing.EffectSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for EffectsWindows.xaml
    /// </summary>
    public partial class EffectsWindow : Window
    {
        private readonly EffectService effectService;
        Microsoft.Win32.OpenFileDialog openFileDialog;
        public EffectsWindow(EffectService effectService)
        {
            InitializeComponent();
            openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL|*.dll"
            };
            this.effectService = effectService;
            effectService.OnEffectsChanged += EffectService_OnEffectsChanged;
        }
        IReadOnlyList<EffectAssembly> Assemblies => effectService.EffectAssemblies;
        private void EffectService_OnEffectsChanged(object arg1, EventArgs arg2)
        {
            treeView.ItemsSource = null;
            treeView.ItemsSource = effectService.EffectAssemblies;
        }

        private void AddProcessorClick(object sender, RoutedEventArgs e)
        {
            if(openFileDialog.ShowDialog() == true && openFileDialog.CheckFileExists)
            {
                Assembly? assembly = null;
                try
                {
                    assembly = Assembly.LoadFile(openFileDialog.FileName);
                }
                catch (BadImageFormatException)
                {
                    MessageBox.Show("فایل قابل خواندن نیست","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if(assembly != null)
                {
                    EffectAssembly effectAssembly = new EffectAssembly(assembly);
                    effectService.Add(effectAssembly);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem == null)
                return;
            if(treeView.SelectedItem is EffectAssembly effectAssembly)
                effectService.Remove(effectAssembly);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EffectAssembly effectAssembly = null;
            if (treeView.SelectedItem is ImageProcessorUnit processorUnit)
                effectAssembly = processorUnit.EffectAssembly;
            else if (treeView.SelectedItem is EffectAssembly effectAssembly1)
                effectAssembly = effectAssembly1;
            else
            {
                nameTextBlock.Text = string.Empty;
                descTextBlock.Text = string.Empty;
                creatorTextBlock.Text = string.Empty;
                return;
            }

            nameTextBlock.Text = effectAssembly.Name;
            descTextBlock.Text = effectAssembly.Description;
            creatorTextBlock.Text = effectAssembly.Creator;

        }
    }
}
