using System;
using System.Windows;

namespace CascadePass.Ariadne
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var args = e.Args;

            if (e is null || e.Args.Length == 0)
            {
                var mainWindow = new MainWindow
                {
                };

                mainWindow.Show();
            }
            else
            {
                Console.WriteLine("Command line arguments detected. Launching with arguments:");
                Console.WriteLine(string.Join(", ", args));
                Console.ReadKey();
            }
        }
    }

}
