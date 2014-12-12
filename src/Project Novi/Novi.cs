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

            // Add event hooks
            this.Paint += Novi_Paint;
            this.MouseClick += Novi_Click;
            this.MouseUp += Novi_MouseUp;
            this.MouseDown += Novi_MouseDown;
            this.MouseMove += Novi_MouseMove;
        }


        private void Novi_Paint(object sender, PaintEventArgs e)
        {
            if (!Visible || View == null)
                return;

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
                var moduleRect = BackgroundView.GetModuleRectangle(Bounds);
                
                // Make the module believe it's drawing at 0,0
                g.TranslateTransform(moduleRect.X, moduleRect.Y);
                moduleRect.X = 0;
                moduleRect.Y = 0;

                // and disallow any drawing outside of the reserved space.
                g.SetClip(moduleRect);
                View.Render(g, moduleRect);
            }
            else
                View.Render(g, windowRectangle);
        }

        private Point scalePoint(Point original)
        {
            var sizeY = Bounds.Height;
            var sizeX = Bounds.Width;
            var scaleX = (float)(1920d / sizeX);
            var scaleY = (float)(1080d / sizeY);
            var scale = Math.Min(scaleX, scaleY);
            sizeY = (int)(scale * original.Y);
            sizeX = (int)(scale * original.X);

            var moduleRect = BackgroundView.GetModuleRectangle(Bounds);
            sizeX -= moduleRect.X;
            sizeY -= moduleRect.Y;
            return new Point(sizeX, sizeY);
        }

        private void Novi_Click(object sender, MouseEventArgs e)
        {
            _controller.HandleTouch(scalePoint(e.Location));
        }

        private void Novi_MouseDown(object sender, MouseEventArgs e)
        {
            _controller.HandleTouchStart(scalePoint(e.Location));
        }

        private void Novi_MouseMove(object sender, MouseEventArgs e)
        {
            _controller.HandleTouchMove(scalePoint(e.Location));
        }

        private void Novi_MouseUp(object sender, MouseEventArgs e)
        {
            _controller.HandleTouchEnd(scalePoint(e.Location));
        }
    }
}
