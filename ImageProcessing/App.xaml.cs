using ImageProcessing.EffectSystem;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }
        [STAThread]
        static void Main(string[] args)
        {
            App app = new App();
            app.InitializeComponent();
            
            app.Run();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices(serviceCollection =>
                {
                    serviceCollection.AddSingleton<MainWindow>();
                    serviceCollection.AddSingleton<EffectsWindow>();
                    serviceCollection.AddSingleton<EffectService>();
                })
                .Build();

            AppHost.Services.GetRequiredService<MainWindow>().Show();
        }
    }
}
