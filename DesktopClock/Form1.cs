using DesktopClock.Properties;

namespace DesktopClock
{
    

    public partial class Form1 : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private ContextMenuStrip contextMenu;

        public Form1()
        {
            InitializeComponent();
            this.MouseClick += Form1_MouseDown;
            contextMenu = new ContextMenuStrip();

            ToolStripMenuItem changeFontMenuItem = new ToolStripMenuItem("Change Font");
            ToolStripMenuItem changeColorMenuItem = new ToolStripMenuItem("Change Text Color");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");

            changeFontMenuItem.Click += ChangeFontMenuItem_Click;
            changeColorMenuItem.Click += ChangeColorMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;

            contextMenu.Items.Add(changeFontMenuItem);
            contextMenu.Items.Add(changeColorMenuItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(exitMenuItem);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.Location = Settings.Default.WindowPosition;
            this.Icon = Properties.Resources.Clock_icon;

            if (!string.IsNullOrEmpty(Settings.Default.FontSetting))
            {
                label1.Font = new FontConverter().ConvertFromString(Settings.Default.FontSetting) as Font;
            }
            if (!string.IsNullOrEmpty(Settings.Default.ColorSetting))
            {
                label1.ForeColor = ColorTranslator.FromHtml(Settings.Default.ColorSetting);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm");
            if (this.Location != Settings.Default.WindowPosition)
            {
                Settings.Default.WindowPosition = this.Location;
                Settings.Default.FontSetting = new FontConverter().ConvertToString(label1.Font);
                Settings.Default.ColorSetting = ColorTranslator.ToHtml(label1.ForeColor);
                Settings.Default.Save();
            }
        }

        private void ChangeFontMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    label1.Font = fontDialog.Font;
                }
            }
        }

        private void ChangeColorMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog()
                    == DialogResult.OK)
                {
                    label1.ForeColor = colorDialog.Color;
                }
            }
        }
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }

            else if (e.Button == MouseButtons.Right)
            {
                contextMenu.Show(this, e.Location); 
            }
        }
    }
}