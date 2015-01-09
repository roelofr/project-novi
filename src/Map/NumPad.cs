using System;
using System.Collections.Generic;
using System.Drawing;

namespace Map
{
    class NumPad
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string[] Values { get; set; }
        public SolidBrush ButtonColor { get; set; }
        public SolidBrush TextColor { get; set; }
        public Font TextFont { get; set; }
        public List<TouchButton> TouchButtons { get; set; }

        public NumPad(int x, int y, int width, int height, string[] values, Color buttoncolor, Color textcolor, Font font)
        {
            Xpos = x;
            Ypos = y;
            Width = width;
            Height = height;
            Values = values;
            ButtonColor = new SolidBrush(buttoncolor);
            TextColor = new SolidBrush(textcolor);
            TextFont = font;
            TouchButtons = new List<TouchButton>();
            CreateNumPad();
        }

        /// <summary>
        /// Create the numpad with all its buttons
        /// </summary>
        public void CreateNumPad()
        {
            //Determine how many rows/columns numpad should use
            var columns = Math.Sqrt(Values.Length * (double)Width / Height);
            var rows = Math.Sqrt(Values.Length * (double)Height / Width);
            if (Math.Ceiling(columns) - columns >= Math.Ceiling(rows) - rows)
            {
                columns = Math.Ceiling(columns);
                rows = Math.Round(rows);
            }
            else
            {
                columns = Math.Round(columns);
                rows = Math.Ceiling(rows);
            }

            // Ensure fitting width/height
            Width = (int)Math.Floor(Width / columns) * (int)columns;
            Height = (int)Math.Floor(Height / rows) * (int)rows;

            // Determine maximum fitting textsize
            var textLength = 0;
            foreach (var s in Values)
            {
                if (s.Length > textLength)
                {
                    textLength = s.Length;
                }
            }
            var textSpace = (int) (Height/rows);
            if ((Width/columns) <= (Height/rows))
            {
                textSpace = (int) (Width/columns);
            }
            TextFont = new Font(TextFont.FontFamily, (int)(0.8 * textSpace / textLength));

            // Create all buttons for numpad
            var rowIndex = 0;
            var columnIndex = 0;
            foreach (var s in Values)
            {
                // Create optional disabled buttons at bottom row to fill up space, left side
                if (rowIndex == (int)(rows - 1))
                {
                    while (TouchButtons.Count < Math.Ceiling((((rows * columns) - Values.Length) / 2) + rowIndex * columns))
                    {
                        var tb = new TouchButton((int)(Xpos + (columnIndex * (Width / columns))),
                            (int)(Ypos + (rowIndex * (Height / rows))), (int)(Width / columns), (int)(Height / rows), " ",
                            ButtonColor.Color, TextColor.Color, TextFont);
                        tb.Enabled = false;
                        TouchButtons.Add(tb);
                        columnIndex++;
                    }

                }

                // Create button
                TouchButtons.Add(new TouchButton((int) (Xpos + (columnIndex*(Width/columns))),
                        (int) (Ypos + (rowIndex*(Height/rows))), (int) (Width/columns), (int) (Height/rows), s,
                        ButtonColor.Color, TextColor.Color, TextFont));

                
                
                if (columnIndex == (int) columns - 1)
                {
                    rowIndex++;
                    columnIndex = 0;
                }
                else
                {
                    columnIndex++;
                    // Create optional disabled buttons at bottom row to fill up space, right side
                    if (rowIndex == (int)(rows - 1) && TouchButtons.Count >= Values.Length + Math.Ceiling(((rows * columns) - Values.Length) / 2))
                    {
                        while (TouchButtons.Count < (int)(rows * columns))
                        {
                            var tb = new TouchButton((int)(Xpos + (columnIndex * (Width / columns))),
                                (int)(Ypos + (rowIndex * (Height / rows))), (int)(Width / columns), (int)(Height / rows), " ",
                                ButtonColor.Color, TextColor.Color, TextFont);
                            tb.Enabled = false;
                            TouchButtons.Add(tb);
                            columnIndex++;
                        }

                    }
                }
                
            }
        }

        /// <summary>
        /// Draw the numpad
        /// </summary>
        /// <param name="graphics"></param>
        public void DrawTouchPad(Graphics graphics)
        {
            foreach (var tb in TouchButtons)
            {
                tb.Draw(graphics);
            }
        }


    }
}
