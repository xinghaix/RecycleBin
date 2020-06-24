using System.Windows;

namespace RecycleBin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly Tray _tray = new Tray();

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            _tray.CreateTrayIcon();
        }

        // 程序退出
        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            _tray.RemoveTrayIcon();
        }
    }
}