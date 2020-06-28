using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RecycleBin
{
    /// <summary>托盘图标</summary>
    public class Tray
    {
        private NotifyIcon _notifyIcon;
        private bool _recycleBinIsEmpty = true;
        private readonly RecycleBinHandle _recycleBinHandle;
        private readonly Language _language = new Language();
        private const string ExeName = "RecycleBin.exe";

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
                Text = _language.Get("RecycleBin"),
                Visible = true
            };

            var autoStartMenuItem = new ToolStripMenuItem(_language.Get("AutoStart"), null, AutoStartHandler);
            var languageMenuItem = new ToolStripMenuItem(_language.Get("Language"), null, (sender, e) => { });
            var openMenuItem = new ToolStripMenuItem(_language.Get("OpenRecycleBin"), null, _recycleBinHandle.Open);
            var clearMenuItem = new ToolStripMenuItem(_language.Get("ClearRecycleBin"), null, _recycleBinHandle.Clear);
            var exitMenuItem = new ToolStripMenuItem(_language.Get("Exit"), null, TrayExit);
            
            // 查看是否已经设置为开机启动
            autoStartMenuItem.Checked = IsAutoStart();

            // 添加菜单
            var contextMenuStrip = new ContextMenuStrip();
            // contextMenuStrip.Items.Add(settingsMenuItem);
            contextMenuStrip.Items.Add(autoStartMenuItem);
            contextMenuStrip.Items.Add(languageMenuItem);
            contextMenuStrip.Items.Add(openMenuItem);
            contextMenuStrip.Items.Add(clearMenuItem);
            contextMenuStrip.Items.Add(exitMenuItem);
            // 设置右键菜单
            _notifyIcon.ContextMenuStrip = contextMenuStrip;

            // 语言设置添加二级菜单
            var supportedLanguages = _language.Get("SupportedLanguages");
            if (!string.IsNullOrEmpty(supportedLanguages))
            {
                var languages = supportedLanguages.Split(char.Parse(","));
                foreach (var language in languages)
                {
                    var languageMenuItemTmp = new ToolStripMenuItem(language, null,
                        (sender, e) =>
                        {
                            if (!(sender is ToolStripMenuItem toolStripMenuItem)) return;
                            // 设置选中
                            toolStripMenuItem.Checked = true;

                            // 修改语言
                            _language.SetLocal(language);

                            foreach (var dropDownItem in languageMenuItem.DropDownItems)
                            {
                                if (dropDownItem.Equals(toolStripMenuItem)) continue;
                                // 其他语言设置为未选中
                                if (dropDownItem is ToolStripMenuItem dropDownItemTmp)
                                    dropDownItemTmp.Checked = false;
                            }

                            contextMenuStrip.Items[0].Text = _language.Get("AutoStart");
                            contextMenuStrip.Items[1].Text = _language.Get("Language");
                            contextMenuStrip.Items[2].Text = _language.Get("OpenRecycleBin");
                            contextMenuStrip.Items[3].Text = _language.Get("ClearRecycleBin");
                            contextMenuStrip.Items[4].Text = _language.Get("Exit");
                        });

                    if (_language.GetLocal() == language) languageMenuItemTmp.Checked = true;

                    languageMenuItem.DropDownItems.Add(languageMenuItemTmp);
                }
            }

            // var settingsMenuItem = new ToolStripMenuItem("设置", null, (sender, e) =>
            // {
            //     if (_trayContextMenu == null)
            //     {
            //         _trayContextMenu = new TrayContextMenu();
            //     }
            //     _trayContextMenu.Visible = true;
            // });

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
            _notifyIcon.Icon =
                isEmpty ? Properties.Resources.RecycleBinEmptyIcon : Properties.Resources.RecycleBinFullIcon;
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

        private static void AutoStartHandler(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem menuItem)) return;

            menuItem.Checked = !menuItem.Checked ? SetAutoStart() : !CancelAutoStart();
        }

        /// <summary>
        /// 通过在用户开机
        /// </summary>
        /// <param name="description">快捷方式描述</param>
        /// <param name="iconLocation">快捷方式图标</param>
        /// <returns>如果设置开机启动成功，那么就返回true</returns>
        private static bool SetAutoStart(string description = null, string iconLocation = null)
        {
            try
            {
                // 存放快捷方式文件的完整路径
                // 获取所有用户的 开始 文件夹位置
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                // 添加引用 Com 中搜索 Windows.Script.Host.Object.Model
                var shortcutPath = Path.Combine(directory, $"{ExeName}.lnk");
                var shell = new IWshRuntimeLibrary.WshShell();
                // 创建快捷方式对象
                var shortcut = (IWshRuntimeLibrary.IWshShortcut) shell.CreateShortcut(shortcutPath);
                // 当前程序完整路径
                var targetPath = Directory.GetCurrentDirectory();
                // 指定目标路径
                shortcut.TargetPath = $"{targetPath}\\{ExeName}";
                // 设置起始位置
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                // 设置运行方式，默认为常规窗口
                shortcut.WindowStyle = 1;
                // 设置备注
                shortcut.Description = description;
                // 设置图标路径
                shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;
                // 保存快捷方式
                shortcut.Save();

                return true;
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        private static bool IsAutoStart()
        {
            var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            return File.Exists($"{startupPath}\\{ExeName}.lnk");
        }

        /// <summary>
        /// 取消开机启动
        /// </summary>
        /// <returns>如果取消开机启动成功，就返回true</returns>
        private static bool CancelAutoStart()
        {
            var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var path = $"{startupPath}\\{ExeName}.lnk";
            File.Delete(path);
            return !File.Exists(path);
        }
    }
}