using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Novi.Modules.Map
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
        private Stopwatch TouchTimer;
        public Stopwatch ActiveTimer;

        private readonly StringFormat _formatText = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public TouchButton(int x, int y, int width, int height, string value, Color buttoncolor, Color textcolor, Font font)
        {
            Xpos = x;
            Ypos = y;
            Width = width;
            Height = height;
            Value = value;
            ButtonColor = new SolidBrush(Color.FromArgb(200, buttoncolor));
            TextColor = new SolidBrush(Color.FromArgb(200, textcolor));
            TextFont = font;
            Enabled = true;
            ActiveTimer = new Stopwatch();
            TouchTimer = new Stopwatch();
        }

        public bool IsClicked(Point p)
        {
            if (p.X >= Xpos && p.X <= Xpos + Width && p.Y >= Ypos && p.Y <= Ypos + Height && Enabled)
            {
                TouchTimer.Start();
                return true;
            }
            else
            {
                return false;
            }
        }


        public void DrawButton(Graphics g)
        {
            if (TouchTimer.ElapsedMilliseconds < 250 && TouchTimer.IsRunning)
            {
                ButtonColor = new SolidBrush(Color.FromArgb(255, ButtonColor.Color));
                TextColor = new SolidBrush(Color.FromArgb(255, TextColor.Color));
            }
            else if (Enabled)
            {
                ButtonColor = new SolidBrush(Color.FromArgb(200, ButtonColor.Color));
                TextColor = new SolidBrush(Color.FromArgb(200, TextColor.Color));
                TouchTimer.Reset();
            }
            else
            {
                ButtonColor = new SolidBrush(Color.FromArgb(100, ButtonColor.Color));
                TextColor = new SolidBrush(Color.FromArgb(100, TextColor.Color));
            }
            var buttonRect = new Rectangle(Xpos, Ypos, Width, Height);
            g.FillRectangle(ButtonColor, buttonRect);
            if (ActiveTimer.IsRunning && Math.Floor((decimal) (ActiveTimer.ElapsedMilliseconds/500))%2 == 0)
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
