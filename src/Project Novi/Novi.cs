using System;
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
    }
}
