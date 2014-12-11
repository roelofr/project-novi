using Project_Novi.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Render.UI
{
    class BasicButton : IButton
    {
        protected static readonly Brush _backgroundDark = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
        protected static readonly Brush _backgroundLight = new SolidBrush(Color.FromArgb(100, 255, 255, 255));

        protected static readonly Brush _textDark = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        protected static readonly Brush _textLight = new SolidBrush(Color.FromArgb(255, 100, 100, 100));

        public Size Size { get; set; }
        public Point Location { get; set; }

        public int FontSize { get; set; }

        public Boolean IsDark { get; set; }

        private IController _controller;

        public event EventHandler Click;
        public BasicButton(IController controller)
        {
            _controller = controller;
            _controller.Touch += controllerTouched;

            // Set default values
            Size = new Size(100, 20);
            Location = new Point(0, 0);
            IsDark = true;

            // Calculate height to points, using 60% of available height.
            FontSize = (int)(Size.Height * 72d / 96d * 0.6);

        }
        /// <summary>
        /// Unregister the handler
        /// </summary>
        ~BasicButton()
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
        protected void DrawText(Graphics graphics, String text)
        {
            var rekt = new Rectangle(Location, Size);
            var fontSize = (int)Math.Floor(Size.Height * 72 / 96 * .7d);
            DrawText(graphics, text, rekt, fontSize);
        }
        protected void DrawText(Graphics graphics, String text, int fontSize)
        {
            var rekt = new Rectangle(Location, Size);
            DrawText(graphics, text, rekt, fontSize);
        }
        protected void DrawText(Graphics graphics, String text, Rectangle location, int fontSize)
        {
            var fg = IsDark ? _textDark : _textLight;
            var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };            
            var font = TextUtils.GetFont(fontSize);

            graphics.DrawString(text, font, fg, location, format);
        }
        protected void DrawBackground(Graphics graphics)
        {
            DrawBackground(graphics, new Rectangle(Location, Size));
        }
        protected void DrawBackground(Graphics graphics, Rectangle location)
        {
            var bg = IsDark ? _backgroundDark : _backgroundLight;
            graphics.FillRectangle(bg, location);

        }
        public virtual void Render(Graphics graphics)
        {
            DrawBackground(graphics);
        }
    }
}
