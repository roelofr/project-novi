using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Modules.Map
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

        public void CreateNumPad()
        {
            
            var columns = Math.Sqrt((double)Values.Length * (double)Width/(double)Height);
            var rows = Math.Sqrt((double)Values.Length * (double)Height / (double)Width);
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
            var textLength = 0;
            foreach (var s in Values)
            {
                if (s.Length > textLength)
                {
                    textLength = s.Length;
                }
            }
            var textSpace = 0;
            if ((Width/columns) <= (Height/rows))
            {
                textSpace = (int) (Width/columns);
            }
            else
            {
                textSpace = (int) (Height/rows);
            }
            TextFont = new Font(TextFont.FontFamily, (int)(0.8 * (textSpace / textLength)));
            var rowIndex = 0;
            var columnIndex = 0;
            foreach (var s in Values)
            {
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
            //while (TouchButtons.Count != (int)(rows * columns))
            //{
            //    var tb = new TouchButton((int)(Xpos + (columnIndex * (Width / columns))), (int)(Ypos + (rowIndex * (Height / rows))), (int)(Width / columns), (int)(Height / rows), " ", ButtonColor.Color, TextColor.Color, TextFont);
            //    tb.Enabled = false;
            //    TouchButtons.Add(tb);
            //    columnIndex++;
            //}
        }

        public void DrawTouchPad(Graphics graphics)
        {
            foreach (var tb in TouchButtons)
            {
                tb.DrawButton(graphics);
            }
        }


    }
}
