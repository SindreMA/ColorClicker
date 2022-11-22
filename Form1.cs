using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using WindowsInput;
using Timer = System.Timers.Timer;
using System.Drawing.Imaging;
using System.Threading;

namespace ColorClicker
{
    public partial class Form1 : Form
    {
        Timer _timer;
        InputSimulator input = new InputSimulator();
        public Form1()
        {
            InitializeComponent();
            _timer = new Timer();
            _timer.Interval = 200;
            _timer.Elapsed += _timer_Elapsed;

            Thread.Sleep(1000);

            var barColor = Color.FromArgb(255, 196, 33, 41);
            colorDialog1.Color = barColor;
            pictureBox2.BackColor = colorDialog1.Color;

            
            _timer_Elapsed(null, null);
        }

        private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            
            int middleW = resolution.Width / 2;
            int middleH = resolution.Height / 2;

            int size = Convert.ToInt32(numericUpDown3.Value);

            Rectangle rect = new Rectangle(middleW - (size / 2), middleH - (size / 2), size, size);


            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            //RGB

            bool ColorsAreClose(Color a, Color z, int threshold = 50)
            {
                int r = (int)a.R - z.R,
                    g = (int)a.G - z.G,
                    b = (int)a.B - z.B;
                return (r * r + g * g + b * b) <= threshold * threshold;
            }

            bool didClick = false;
            for (int x = 0; x < size; x++)
            {
                if (didClick) break;
                for (int y = 0; y < size; y++)
                {
                    var copy = bmp.Clone() as Bitmap;
                    Color pixel = copy.GetPixel(x, y);

                    if (ColorsAreClose(pixel, colorDialog1.Color, Convert.ToInt32(numericUpDown1.Value)))
                    {
                        if (e != null) input.Mouse.LeftButtonClick();
                        didClick = true;
                        break;
                    }
                }
            }

            pictureBox1.Image = bmp.Clone() as Image;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                button1.Text = "Start";
            } else
            {
                _timer.Start();
                button1.Text = "Stop";
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                colorDialog1.Color = colorDialog1.Color;
                pictureBox2.BackColor = colorDialog1.Color;
            }
        }
    }
}
