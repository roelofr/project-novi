using System;
using System.Drawing;
using System.Windows.Forms;
using Project_Novi.Api;

namespace Project_Novi
{
    public partial class Novi : Form
    {
        private readonly NoviController _controller;
        internal IView View { get; set; }

        internal IBackgroundView BackgroundView { get; set; }

        internal Novi(NoviController controller)
        {
            InitializeComponent();
            DoubleBuffered = true;
            _controller = controller;
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

            var windowRectangle = new Rectangle(0, 0, 1920, 1080);

            if (BackgroundView != null)
            {
                BackgroundView.Render(g, windowRectangle);
                View.Render(g, BackgroundView.GetModuleRectangle(windowRectangle));
            }
            else
                View.Render(g, windowRectangle);
        }

        private void Novi_Click(object sender, MouseEventArgs e)
        {
            var sizeY = Bounds.Height;
            var sizeX = Bounds.Width;
            var scaleX = (float)(1920d / sizeX);
            var scaleY = (float)(1080d / sizeY);
            var scale = Math.Min(scaleX, scaleY);
            sizeY = (int)(scale * e.Location.Y);
            sizeX = (int)(scale * e.Location.X);

            _controller.HandleTouch(new Point(sizeX, sizeY));
        }
    }
}
