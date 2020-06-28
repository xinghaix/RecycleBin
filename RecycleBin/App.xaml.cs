using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace RecycleBin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static Mutex _mutex;
        private readonly Tray _tray = new Tray();

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            CheckRunning();
            
            _tray.CreateTrayIcon();
        }

        /// <summary>
        /// 检测是否已经有相同程序在运行。如果存在，则退出当前程序
        /// </summary>
        private static void CheckRunning()
        {
            bool noRun;
            _mutex = new Mutex(true, "RecycleBin", out noRun);

            if (noRun) _mutex.ReleaseMutex();
            else Process.GetCurrentProcess().Kill();
        }

        // 程序退出
        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            _tray.RemoveTrayIcon();
        }
    }
}