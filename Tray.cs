using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RecycleBin
{
    /// <summary>托盘图标</summary>
    public class Tray
    {
        private NotifyIcon _notifyIcon;
        private readonly RecycleBinHandle _recycleBinHandle;
        private bool _recycleBinIsEmpty = true;
        private TrayContextMenu _trayContextMenu = new TrayContextMenu();

        public Tray()
        {
            _recycleBinHandle = new RecycleBinHandle();
        }

        public void CreateTrayIcon()
        {
            if (_notifyIcon != null) return;

            _notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.RecycleBinEmptyIcon,
                Text = @"回收站",
                Visible = true
            };
            
            // ContextMenu和MenuItem在.net3.1之后的版本将不可用
            // var openMenuItem = new MenuItem("打开");
            // var contextMenu = new ContextMenu();
            // contextMenu.MenuItems.Add(openMenuItem);
            // _notifyIcon.ContextMenu = contextMenu;
            var openMenuItem = new ToolStripMenuItem("打开回收站", null, _recycleBinHandle.Open);
            var clearMenuItem = new ToolStripMenuItem("清空回收站", null, _recycleBinHandle.Clear);
            var exitMenuItem = new ToolStripMenuItem("退出", null, TrayExit);
            // var settingsMenuItem = new ToolStripMenuItem("设置", null, (sender, e) =>
            // {
            //     if (_trayContextMenu == null)
            //     {
            //         _trayContextMenu = new TrayContextMenu();
            //     }
            //     _trayContextMenu.Visible = true;
            // });
            //
            // 添加菜单
            var contextMenuStrip = new ContextMenuStrip();
            // contextMenuStrip.Items.Add(settingsMenuItem);
            contextMenuStrip.Items.Add(openMenuItem);
            contextMenuStrip.Items.Add(clearMenuItem);
            contextMenuStrip.Items.Add(exitMenuItem);
            // 设置右键菜单
            _notifyIcon.ContextMenuStrip = contextMenuStrip;

            // 托盘图标添加鼠标双击事件
            _notifyIcon.MouseDoubleClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left) _recycleBinHandle.Clear(sender, e);
            };

            // 实时监听回收站是否存在文件，如果存在就修改托盘图标显示
            _recycleBinHandle.StartTimer(HandleTrayIcon);
        }

        // 处理图标显示
        private void HandleTrayIcon(bool isEmpty)
        {
            if (_recycleBinIsEmpty == isEmpty) return;
                
            _recycleBinIsEmpty = isEmpty;
            _notifyIcon.Icon = isEmpty ?
                Properties.Resources.RecycleBinEmptyIcon :
                Properties.Resources.RecycleBinFullIcon; 
        }

        // 退出程序
        private void TrayExit(object sender, EventArgs e)
        {
            RemoveTrayIcon();
            Process.GetCurrentProcess().Kill();
        }

        // 释放资源
        public void RemoveTrayIcon()
        {
            if (_notifyIcon == null) return;

            _notifyIcon.Visible = false;
            // 释放资源
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }
    }
}