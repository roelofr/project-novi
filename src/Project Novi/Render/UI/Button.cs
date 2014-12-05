using Project_Novi.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Render.UI
{
    class Button : IButton
    {
        private static readonly Brush _backgroundDark = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
        private static readonly Brush _backgroundLight = new SolidBrush(Color.FromArgb(100, 255, 255, 255));

        private static readonly Brush _textDark = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        private static readonly Brush _textLight = new SolidBrush(Color.FromArgb(255, 100, 100, 100));

        public Size Size { get; set; }
        public Point Location { get; set; }
        public String Text { get; set; }

        public int FontSize { get; set; }

        public Boolean IsDark { get; set; }

        private IController _controller;

        public event EventHandler Click;
        public Button(IController controller) {
            _controller = controller;
            _controller.Touch += controllerTouched;

            // Set default values
            Size = new Size(100, 20);
           Location = new Point(0, 0);
            Text = "Button";

            // Calculate height to points, using 60% of available height.
            FontSize = (int) (Size.Height * 72d / 96d * 0.6);

        }
        /// <summary>
        /// Unregister the handler
        /// </summary>
        ~Button()
        {
            if (_controller != null)
                _controller.Touch -= controllerTouched;
        }
        private bool shouldClick(Point point)
        {
            var rect = new Rectangle(Location, Size);
            var pointRect = new Rectangle(point, new Size(1, 1));

            return rect.IntersectsWith(pointRect);

        }
        public void controllerTouched(Point point)
        {
            if (!shouldClick(point))
                return;

            if (this.Click != null)
                this.Click(this, new EventArgs());

        }
        public void Render(Graphics graphics)
        {
            var bg = IsDark ? _backgroundDark : _backgroundLight;
            var fg = IsDark ? _textDark : _textLight;

            var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            var rect = new Rectangle(Location, Size);

            var font = TextUtils.GetFont(FontSize);

            graphics.FillRectangle(bg, rect);
            graphics.DrawString(Text, font, fg, rect, format);
            
        }
    }
}
