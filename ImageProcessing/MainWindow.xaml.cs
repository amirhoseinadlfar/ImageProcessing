using ImageProcessing.Base;
using ImageProcessing.Base.Effects;
using ImageProcessing.EffectSystem;
using ImageProcessing.ImageRenderers;

using Microsoft.Win32;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Win32.OpenFileDialog fileDialog;
        SaveFileDialog saveFileDialog;
        private readonly EffectService effectService;
        private readonly EffectsWindow effectsWindow;
        Bitmap biti = null;
        Timer renderProgressTimer;
        IRender? renderer;
        Stopwatch timeThing = new Stopwatch();
        public MainWindow(EffectService effectService, EffectsWindow effectsWindow)
        {
            InitializeComponent();

            img = (host.Child as System.Windows.Forms.PictureBox)!;
            fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.png;*.BMP;*.JPG;|All files (*.*)|*.*\r\n\r\n"
            };
            this.effectService = effectService;
            this.effectsWindow = effectsWindow;
            effectService.OnEffectsChanged += EffectService_OnEffectsChanged;
            ReloadImageProcessors();
            renderProgressTimer = new Timer();
            renderProgressTimer.Interval = 1000/20;
            renderProgressTimer.Elapsed += RenderProgressTimer_Elapsed;
            var picTypeMode = typeof(System.Windows.Forms.PictureBoxSizeMode);
            
            
            viewCombo.ItemsSource = Enum.GetValues(picTypeMode);
            viewCombo.SelectedIndex = 0;
        }
        bool dou = false;
        private void RenderProgressTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (renderer is null)
            {
                renderProgressTimer.Stop();
                return;
            }
            Dispatcher.Invoke(() =>
            {
                prog.Value = renderer.CalculateComp(true);
                if (prog.Value == 100)
                {
                    progLbl.Content = $"انجام شد در {timeThing.Elapsed.Seconds}s {timeThing.Elapsed.Milliseconds}ms";
                    renderProgressTimer.Stop();
                    biti = ConvertToBitmap(renderer.GetOutput());
                    img.Image = biti.Clone() as System.Drawing.Bitmap;
                    timeThing.Stop();
                    timeThing.Restart();
                    processBtn.IsEnabled = true;
                    return;
                }
                progLbl.Content = $"در حال انجام ({timeThing.Elapsed})";
                if(dou == true)
                {
                    biti = ConvertToBitmap(renderer.GetOutput());
                    img.Image = biti.Clone() as System.Drawing.Bitmap;
                    dou = false;
                }
                else dou = true;

            });
        }

        private void EffectService_OnEffectsChanged(object arg1, EventArgs arg2)
        {
            ReloadImageProcessors();
        }
        void ReloadImageProcessors()
        {
            modeCombo.DisplayMemberPath = nameof(ImageProcessorUnit.Name);
            modeCombo.ItemsSource = null;
            modeCombo.ItemsSource = effectService.AllImageProcessorUnits;
        }

        private void ImageSelectClick(object sender, RoutedEventArgs e)
        {
            if(fileDialog.ShowDialog() == true && fileDialog.CheckFileExists)
            {
                biti = new Bitmap(fileDialog.FileName);
                OnImageSelect();
            }
        }
        System.Windows.Forms.PictureBox img;
        private void OnImageSelect()
        {
            if (biti is null)
                return;
            Bitmap bitiClone = biti.Clone() as Bitmap;
            var b = ConvertToByteArray(bitiClone);
            img.Image = ConvertToBitmap(b);
            Height++;
            
            
        }
        static byte[,,] ConvertToByteArray(Bitmap bitmap)
        {
            var imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var depth = Bitmap.GetPixelFormatSize(PixelFormat.Format32bppArgb);
            
            byte[,,] bytes = new byte[bitmap.Width,bitmap.Height,4];
            for(int x = 0;x<bytes.GetLength(0);x++)
                for (int y = 0; y < bytes.GetLength(1); y++)
                {
                    int cCount = depth / 8;
                    int p = ((y * imageData.Width) + x) * cCount;
                    unsafe
                    {
                        byte* imgPtr = (byte*)imageData.Scan0 + p; 
                        bytes[x, y, 3] = imgPtr[0];
                        bytes[x, y, 2] = imgPtr[1];
                        bytes[x, y, 1] = imgPtr[2];
                        bytes[x, y, 0] = imgPtr[3];
                    }
                }
            bitmap.UnlockBits(imageData);
            return bytes;
        }
        static Bitmap ConvertToBitmap(byte[,,] bytes)
        {
            var bitmap = new Bitmap(bytes.GetLength(0),bytes.GetLength(1));
            var imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,PixelFormat.Format32bppArgb);
            var depth = Bitmap.GetPixelFormatSize(PixelFormat.Format32bppArgb);


            for (int x = 0;x<bitmap.Width;x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    unsafe
                    {
                        int cCount = depth / 8;
                        int p = ((y * imageData.Width) + x) * cCount;

                        byte* imgPtr = (byte*)imageData.Scan0 + p;

                        imgPtr[0] = bytes[x, y, 3];
                        imgPtr[1] = bytes[x, y, 2];
                        imgPtr[2] = bytes[x, y, 1];
                        imgPtr[3] = bytes[x, y, 0];
                        
                    }
                }
            }
            bitmap.UnlockBits(imageData);
            return bitmap;
        }
        private void OnProcessClick(object sender, RoutedEventArgs e)
        {
            if (biti is null)
                return;
            byte threadCount = 0;
            if (byte.TryParse(threadsTxt.Text, out threadCount) == false)
            {
                MessageBox.Show("تعداد پرادزش صحیح نیست");
                return;
            }
            var selectedItem = modeCombo.SelectedItem;
            if(selectedItem is ImageProcessorUnit unit)
            {
                var instnace = unit.CreateInstance();
                foreach (var item in dictionary)
                {
                    if(item.Key.PropertyInfo.PropertyType == typeof(bool))
                    {
                        if (item.Value is CheckBox checkBox)
                            item.Key.PropertyInfo.SetValue(instnace.Effect, checkBox.IsChecked);
                    }
                    else if(item.Key.PropertyInfo.PropertyType.GetInterfaces().Any(x=>x==typeof(IConvertible)))
                    {
                        if(item.Value is TextBox txtBox)
                        {
                            object value = null;
                            try
                            {
                                value = Convert.ChangeType(txtBox.Text, item.Key.PropertyInfo.PropertyType);
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show($"مقدار فیلد وارد شده قابل قبول نیست.");
                                return;
                            }
                            item.Key.PropertyInfo.SetValue(instnace.Effect, value);
                        }
                    }
                    else if(item.Key.PropertyInfo.PropertyType.IsEnum)
                    {
                        if(item.Value is ComboBox comboBox)
                        {
                            if(selectedItem is EnumProMember mem)
                            {
                                item.Key.PropertyInfo.SetValue(instnace.Effect, mem.Value);
                            }
                        }
                    }
                }
                MultiThreadRenderMode threadRenderMode = MultiThreadRenderMode.Task;
                if (calcuMethodCombo.SelectedIndex == 1)
                    threadRenderMode = MultiThreadRenderMode.Thread;

                if (unit.EffectType == EffectType.Area)
                    renderer = new AreaRender(instnace, threadRenderMode, threadCount);
                else
                    renderer = new PixelRender(instnace, threadRenderMode, threadCount);
                renderer.SetImage(ConvertToByteArray(biti));
                processBtn.IsEnabled = false;
                timeThing.Restart();
                timeThing.Start();
                renderer.StartRender();
                renderProgressTimer.Start();
            }

        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (biti is null) return;
            saveFileDialog ??= new SaveFileDialog()
            {
                Filter = "PNG|*.png"
            };
            if(saveFileDialog.ShowDialog() == true)
            {
                using FileStream fileC = File.Create(saveFileDialog.FileName);
                biti.Save(fileC,ImageFormat.Jpeg);
            }
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        private void OnEffectsClick(object sender, RoutedEventArgs e)
        {
            effectsWindow.ShowDialog();
        }
        Dictionary<ImageProcessorProperty, UIElement> dictionary;
        private void OnEffectSelect(object sender, SelectionChangedEventArgs e)
        {

            if (modeCombo.SelectedItem == null)
                return;
            ImageProcessorUnit unit = (modeCombo.SelectedItem as ImageProcessorUnit)!;
            effectDesc.Text = unit.Description;
            propertysGrid.Children.Clear();
            propertysGrid.RowDefinitions.Clear();
            dictionary = new Dictionary<ImageProcessorProperty, UIElement>();
            for (int i = 0; i < unit.Properties.Length; i++)
            {

                Type pType = unit.Properties[i].PropertyInfo.PropertyType;
                
                if (pType == typeof(bool))
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.Content = unit.Properties[i].Name;
                    if (string.IsNullOrWhiteSpace(unit.Properties[i].Description) == false)
                        checkBox.ToolTip = new ToolTip()
                        {
                            Content = unit.Properties[i].Description
                        };
                    Grid.SetRow(checkBox, i);
                    Grid.SetColumn(checkBox, 0);
                    Grid.SetColumnSpan(checkBox, 2);
                    propertysGrid.Children.Add(checkBox);
                    dictionary.Add(unit.Properties[i], checkBox);
                }
                else if (pType.GetInterfaces().Any(x=>x == typeof(IConvertible)))
                {
                    AddProLabel();
                    TextBox txtBox = AddProTextBox();
                    dictionary.Add(unit.Properties[i], txtBox);
                }
                else if(pType.IsEnum)
                {
                    string[] optionsNames = Enum.GetNames(pType);
                    var values = pType.GetMembers()
                        .Where(x => x.DeclaringType == pType && optionsNames.Contains(x.Name))
                        .Select(x=>
                        {
                            
                            var displayNameAtt = x.GetCustomAttribute<DisplayAttribute>();
                            EnumProMember enumProMember = new EnumProMember()
                            {
                                Name = displayNameAtt?.Name ?? x.Name,
                                Value = (x as FieldInfo)!.GetValue(null)!
                            };
                            return enumProMember;
                        })
                        .ToArray();
                    AddProLabel();
                    ComboBox comboBox = new ComboBox();
                    comboBox.ItemsSource = values;
                    comboBox.DisplayMemberPath = nameof(EnumProMember.Name);
                    Grid.SetRow(comboBox, i);
                    Grid.SetColumn(comboBox, 1);
                    propertysGrid.Children.Add(comboBox);
                    dictionary.Add(unit.Properties[i], comboBox);
                }
                else return;
                propertysGrid.RowDefinitions.Add(new RowDefinition()
                {

                });
                Label AddProLabel()
                {
                    Label pNameLabel = new Label();
                    pNameLabel.Content = unit.Properties[i].Name;
                    if (string.IsNullOrWhiteSpace(unit.Properties[i].Description) == false)
                    {
                        pNameLabel.ToolTip = new ToolTip()
                        {
                            Content = unit.Properties[i].Description
                        };
                    }
                    Grid.SetColumn(pNameLabel, 0);
                    Grid.SetRow(pNameLabel, i);
                    propertysGrid.Children.Add(pNameLabel);
                    return pNameLabel;
                }
                TextBox AddProTextBox()
                {
                    TextBox textBox = new TextBox();
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, 1);
                    propertysGrid.Children.Add(textBox);
                    return textBox;
                }
                
            }
            
        }

        private void viewCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            img.SizeMode = (System.Windows.Forms.PictureBoxSizeMode)viewCombo.SelectedItem;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
    class EnumProMember
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
