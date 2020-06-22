using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RecycleBin
{
    // 回收站的一些操作
    public class RecycleBinHandle : Form
    {
        // 不显示确认删除的对话框
        private const int SherbNoconfirmation = 0x000001;

        // 不显示删除过程的进度条
        private const int SherbNoprogressui = 0x000002;

        // 当删除完成时，不播放声音
        private const int SherbNosound = 0x000004;

        private System.Timers.Timer _timer;

        [DllImport("shell32.dll")]
        private static extern int SHEmptyRecycleBin(IntPtr handle, string root, int flags);
        
        private static Shqueryrbinfo shqueryrbinfo;

        [DllImport("shell32.dll")]
        private static extern int SHQueryRecycleBin(string pszRootPath, ref Shqueryrbinfo queryInfo);

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct Shqueryrbinfo
        {
            public int cbSize;
            public long i64Size;
            public long i64NumItems;
        }

        public static int GetCount()
        {
            shqueryrbinfo.cbSize = Marshal.SizeOf(typeof(Shqueryrbinfo));
            SHQueryRecycleBin(string.Empty, ref shqueryrbinfo);
            return (int) shqueryrbinfo.i64NumItems;
        }

        // 回收站是否为空
        public bool IsEmpty()
        {
            return GetCount() == 0;
        }

        // 打开回收站（垃圾箱）
        public void Open(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "shell:RecycleBinFolder");
        }

        // 点击"清空回收站"按钮
        public void Clear(object sender, EventArgs e)
        {
            SHEmptyRecycleBin(Handle, "", SherbNoconfirmation + SherbNoprogressui + SherbNosound);
        }

        // 每隔一秒查看回收站是否存在文件
        public void StartTimer(Action<bool> action)
        {
            if (_timer == null)
            {
                _timer = new System.Timers.Timer {Interval = 1200, Enabled = true};
                _timer.Elapsed += delegate { action.Invoke(IsEmpty()); };
            }

            _timer.Start();
        }
    }
}