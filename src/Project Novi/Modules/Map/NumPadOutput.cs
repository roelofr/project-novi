using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Modules.Map
{
    class NumPadOutput
    {
        public NumPad NumPad { get; set; }
        public int ActiveDigit { get; set; }
        public int Digits { get; set; }
        public List<TouchButton> TouchButtons { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Font TextFont { get; set; }
        public string Output { get; set; }
        public NumPadOutput(int x, int y, int width, int height, int digits, NumPad numpad)
        {
            Output = "";
            Xpos = x;
            Ypos = y;
            Width = width;
            Height = height;
            NumPad = numpad;
            Digits = digits;
            var textLength = 0;
            foreach (var s in NumPad.Values)
            {
                if (s.Length > textLength)
                {
                    textLength = s.Length;
                }
            }
            var textSpace = 0;
            if ((Width / Digits) <= Height)
            {
                textSpace = (int)(Width / Digits);
            }
            else
            {
                textSpace = Height;
            }
            TextFont = new Font(NumPad.TextFont.FontFamily, (int)(0.8 * (textSpace / textLength)));
            TouchButtons = new List<TouchButton>();
            ActiveDigit = 0;
            CreateNumPadOutput();
            SetActive(ActiveDigit);
        }

        public void CreateNumPadOutput()
        {
            for (var digit = 0; digit < Digits; digit++)
            {
                TouchButtons.Add(new TouchButton(Xpos + (digit * (Width / Digits)), Ypos, (Width / Digits), Height, "_", NumPad.ButtonColor.Color, NumPad.TextColor.Color, TextFont));
            }
        }

        public void DrawNumPadOutput(Graphics graphics)
        {
            foreach (var tb in TouchButtons)
            {
                tb.DrawButton(graphics);
            }
        }

        public void SetActive(int digit)
        {
            TouchButtons.ElementAt(ActiveDigit).ActiveTimer.Reset();
            ActiveDigit = digit;
            TouchButtons.ElementAt(ActiveDigit).ActiveTimer.Start();
        }

        public void AddOutputDigit()
        {
            if (ActiveDigit != Digits - 1)
            {
                SetActive(ActiveDigit + 1);
            }
            else
            {
                TouchButtons.ElementAt(ActiveDigit).ActiveTimer.Reset();
            }
            
        }

        public void DeleteOutputDigit()
        {
            if (!TouchButtons.ElementAt(ActiveDigit).Value.Equals("_"))
            {
                TouchButtons.ElementAt(ActiveDigit).ActiveTimer.Start();
            } 
            else if (ActiveDigit == Digits - 1)
            {
                if (TouchButtons.ElementAt(ActiveDigit).Value.Equals("_"))
                {
                    SetActive(ActiveDigit - 1);
                }
                TouchButtons.ElementAt(ActiveDigit).ActiveTimer.Start();
            }
            else if (ActiveDigit > 0)
            {
                SetActive(ActiveDigit - 1);
            }
        }

        public void BuildOutput()
        {
            Output = "";
            foreach (var tb in TouchButtons)
            {
                if (!tb.Value.Equals("_") && !tb.ActiveTimer.IsRunning)
                {
                  Output += tb.Value;  
                }
            }
        }

        public void ClearOutput(int digit)
        {
            foreach (var tb in TouchButtons)
            {
                if (TouchButtons.IndexOf(tb) >= digit)
                {
                    tb.Value = "_";
                }
            }
            if (digit == 0)
            {
                AddOutputDigit();
            }
            
        }

    }
}
