using System;
using System.Drawing;
using System.Windows.Forms;
using Project_Novi.Modules;

namespace Project_Novi
{
    public partial class Novi : Form
    {
        private NoviController _controller;
        internal IView View { get; set; }

        public Novi()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Hide();
            var splash = new Splash();
            splash.ShowDialog();
            Show();
            _controller = new NoviController(this);
        }

        private void Novi_Paint(object sender, PaintEventArgs e)
        {
            if (!Visible) return;

            var rect = e.ClipRectangle;
            var g = e.Graphics;

            // Automatically scale drawing to the size of the form
            var scaleX = (float)(rect.Width / 1920d);
            var scaleY = (float)(rect.Height / 1080d);
            var scale = Math.Min(scaleX, scaleY);
            g.ScaleTransform(scale, scale);

            View.Render(g, new Rectangle(0, 0, 1920, 1080));
        }

        private void Novi_Click(object sender, MouseEventArgs e)
        {
            _controller.HandleTouch(e.Location);
        }
    }
}
