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

        public Form1()
        {
            InitializeComponent();
            this.MouseClick += Form1_MouseDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.Location = Settings.Default.WindowPosition;
            this.Icon = Properties.Resources.Clock_icon;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm");
            if (this.Location != Settings.Default.WindowPosition)
            {
                Settings.Default.WindowPosition = this.Location;
                Settings.Default.Save();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
            if (e.Button == MouseButtons.Right)
            {
                Thread.Sleep(200);
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}