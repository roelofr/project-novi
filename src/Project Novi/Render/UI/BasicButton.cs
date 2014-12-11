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
        
        /// <summary>
        /// Holds the user-set value of the text size. Set to -1 to auto-determine the font size
        /// </summary>
        private int _textSize = -1;

        /// <summary>
        /// A link to the controller, so a click event can be fired when required
        /// </summary>
        private IController _controller;

        /// <summary>
        /// The size of this button. This is used for standard drawing and for handling click events
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// Where this button is located. This is used for standard drawing and for handling click events
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// The size at which the text is drawn. Set this to a value <= 0 to automatically size the text to the available height.
        /// Always returns a usable text size. if the size is set to auto determine, the returned value is the calculated size.
        /// </summary>
        public int TextSize
        {
            get
            {
                if (_textSize == -1)
                    return calculateFontSize();
                else
                    return _textSize;
            }
            set
            {
                if(value <= 0)
                    _textSize = -1;
                else
                    _textSize = value;
            }
        }
        /// <summary>
        /// Set to true to make this a dark button
        /// </summary>
        public Boolean IsDark { get; set; }

        /// <summary>
        /// Event that gets fired when the button is clicked
        /// </summary>
        public event EventHandler Click;
        /// <summary>
        /// Creates a new button, the controller is required for click event handling
        /// </summary>
        /// <param name="controller"></param>
        public BasicButton(IController controller)
        {
            _controller = controller;
            _controller.Touch += controllerTouched;

            // Set default values
            Size = new Size(100, 20);
            Location = new Point(0, 0);
            IsDark = true;

        }
        /// <summary>
        /// Destructor, used to unregister the handler
        /// </summary>
        ~BasicButton()
        {
            if (_controller != null)
                _controller.Touch -= controllerTouched;
        }
        /// <summary>
        /// Check if this click event should trigger this button's Click event
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool shouldClick(Point point)
        {
            var rect = new Rectangle(Location, Size);
            var pointRect = new Rectangle(point, new Size(1, 1));

            return rect.IntersectsWith(pointRect);

        }
        /// <summary>
        /// Calculates the font size by this button's height.
        /// </summary>
        /// <returns></returns>
        private int calculateFontSize() {
            return (int)Math.Floor(Size.Height * 72 / 96 * .7d);
        }
        /// <summary>
        /// Draws the text usgin the given Graphics and with the given Text
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        protected void DrawText(Graphics graphics, String text)
        {
            var rekt = new Rectangle(Location, Size);
            DrawText(graphics, text, rekt, TextSize);
        }
        /// <summary>
        /// Draws the text using the given Graphics, with the given Text and at the given FontSize
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        protected void DrawText(Graphics graphics, String text, int fontSize)
        {
            var rekt = new Rectangle(Location, Size);
            DrawText(graphics, text, rekt, fontSize);
        }
        /// <summary>
        /// Draws the text using the given Graphics, with the given Text and at the given FontSize. The rectangle will be used as location, instead of this button's rectangle
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <param name="fontSize"></param>
        protected void DrawText(Graphics graphics, String text, Rectangle location, int fontSize)
        {
            var fg = IsDark ? _textDark : _textLight;
            var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };            
            var font = TextUtils.GetFont(fontSize);

            graphics.DrawString(text, font, fg, location, format);
        }
        /// <summary>
        /// Fills the background of this button
        /// </summary>
        /// <param name="graphics"></param>
        protected void DrawBackground(Graphics graphics)
        {
            DrawBackground(graphics, new Rectangle(Location, Size));
        }
        /// <summary>
        /// Fills the background of the given rectangle
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="location"></param>
        protected void DrawBackground(Graphics graphics, Rectangle location)
        {
            var bg = IsDark ? _backgroundDark : _backgroundLight;
            graphics.FillRectangle(bg, location);

        }
        /// <summary>
        /// Called when the controller triggers a touch event.
        /// </summary>
        /// <param name="point"></param>
        public void controllerTouched(Point point)
        {
            if (!shouldClick(point))
                return;

            if (this.Click != null)
                this.Click(this, new EventArgs());

        }
        /// <summary>
        /// Renders the default button, with the dark background and no text
        /// </summary>
        /// <param name="graphics"></param>
        public void Render(Graphics graphics)
        {
            DrawBackground(graphics);
        }
    }
}
