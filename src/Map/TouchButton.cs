using System.Diagnostics;
using System.Drawing;

namespace Map
{
    class TouchButton
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Value { get; set; }
        public SolidBrush ButtonColor { get; set; }
        public SolidBrush TextColor { get; set; }
        public Font TextFont { get; set; }
        public bool Enabled { get; set; }
        private readonly Stopwatch _touchTimer;
        public Stopwatch ActiveTimer;
        private const int EnabledButton = 150;
        private const int DisabledButton = 100;
        private const int TouchedButton = 200;

        private readonly StringFormat _formatText = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public TouchButton(int x, int y, int width, int height, string value, Color buttoncolor, Color textcolor, Font font)
        {
            Xpos = x;
            Ypos = y;
            Width = width;
            Height = height;
            Value = value;
            ButtonColor = new SolidBrush(Color.FromArgb(EnabledButton, buttoncolor));
            TextColor = new SolidBrush(Color.FromArgb(EnabledButton, textcolor));
            TextFont = font;
            Enabled = true;
            ActiveTimer = new Stopwatch();
            _touchTimer = new Stopwatch();
        }

        /// <summary>
        /// Determines if button is clicked
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsClicked(Point p)
        {
            if (p.X >= Xpos && p.X <= Xpos + Width && p.Y >= Ypos && p.Y <= Ypos + Height && Enabled)
            {
                _touchTimer.Start();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Draw the button
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            if (_touchTimer.ElapsedMilliseconds < 250 && _touchTimer.IsRunning)
            {
                ButtonColor = new SolidBrush(Color.FromArgb(TouchedButton, ButtonColor.Color));
                TextColor = new SolidBrush(Color.FromArgb(TouchedButton, TextColor.Color));
            }
            else if (Enabled)
            {
                ButtonColor = new SolidBrush(Color.FromArgb(EnabledButton, ButtonColor.Color));
                TextColor = new SolidBrush(Color.FromArgb(EnabledButton, TextColor.Color));
                _touchTimer.Reset();
            }
            else
            {
                ButtonColor = new SolidBrush(Color.FromArgb(DisabledButton, ButtonColor.Color));
                TextColor = new SolidBrush(Color.FromArgb(DisabledButton, TextColor.Color));
            }
            var buttonRect = new Rectangle(Xpos, Ypos, Width, Height);
            g.FillRectangle(ButtonColor, buttonRect);
            if (ActiveTimer.IsRunning && ActiveTimer.ElapsedMilliseconds / 500 % 2 == 0 && !_touchTimer.IsRunning)
            {
                g.DrawString(" ", TextFont, TextColor, buttonRect, _formatText);
            }
            else
            {
                g.DrawString(Value, TextFont, TextColor, buttonRect, _formatText);
            }
            
        }
    }
}
