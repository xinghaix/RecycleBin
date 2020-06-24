using System.Windows.Forms;

namespace RecycleBin
{
    /// <summary>
    /// 自定义的托盘菜单
    /// </summary>
    public class TrayContextMenu : Form
    {
        private Button button1;
        private Button button2;
        private Button button4;
        private Button button3;

        public TrayContextMenu()
        {
            InitializeComponent();
            Visible = false;
        }

        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(1, 1);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "设置";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(1, 30);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "打开回收站";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(1, 59);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(75, 23);
            button3.TabIndex = 3;
            button3.Text = "清空回收站";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(1, 88);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(75, 23);
            button4.TabIndex = 4;
            button4.Text = "退出";
            button4.UseVisualStyleBackColor = true;
            // 
            // TrayContextMenu
            // 
            ClientSize = new System.Drawing.Size(77, 112);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "TrayContextMenu";
            Load += TrayContextMenu_Load;
            ResumeLayout(false);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
        }

        private void TrayContextMenu_Load(object sender, System.EventArgs e)
        {
        }
    }
}