using System;
using System.Windows.Forms;
using Project_Novi.Modules;
using System.Drawing;

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

            View.Render(g, rect);
        }

        private void Novi_Click(object sender, MouseEventArgs e)
        {
            var sizeY = this.Bounds.Height;
            var sizeX = this.Bounds.Width;
            var scaleX = (float)(1920d / sizeX);
            var scaleY = (float)(1080d / sizeY);
            var scale = Math.Min(scaleX, scaleY);
            sizeY = (int)(scaleY * e.Location.Y);
            sizeX = (int)(scaleX * e.Location.X);

            _controller.HandleTouch(new Point(sizeX, sizeY));
        }
    }
}
