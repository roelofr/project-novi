using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private enum Floor
        {
            T0, T1, T2, T3, T4, T5
        }

        private readonly MapModule _module;
        private readonly IController _controller;
        private string _activeFloor = "T0";
        List<Rectangle> floor_buttons;
        Point[] driehoek = new Point[3];
        SolidBrush brushActiveFloor = new SolidBrush(Color.Orange);
        SolidBrush brushFloors = new SolidBrush(Color.Blue);

        List<DigitSelector> digit_selectors;
        SolidBrush button_brush = new SolidBrush(Color.Blue);
        SolidBrush text_brush = new SolidBrush(Color.Black);
        Font text_font = new Font("Segoe UI", 16);
        Rectangle ok_button = new Rectangle(100, 150, 50, 50);
      
        string[] verdiepingen = { "0", "1", "2", "3", "4", "5" };
        string[] lokalen = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] bonus = { "a", "b", "c" };
        string[] floors = { "T5", "T4", "T3", "T2", "T1", "T0" };

        private int xpos_floor_buttons = 1650;
        private int ypos_floor_buttons = 300;
        private int width_floor_buttons = 220;
        private int height_floor_buttons = 120;
        private int margin_floor_buttons = 10;
        private int number_floor_buttons = 6;

        struct ArrowButton
        {
            public Point[] coords { get; set; }
            public string direction { get; set; }
        }

        struct DigitBox
        {
            public Point coords { get; set; }
            public string[] values { get; set; }
            public int index { get; set; }
        }

        struct DigitSelector
        {
            public List<ArrowButton> buttons { get; set; }
            public DigitBox digitbox { get; set; }
        }

        public IModule Module
        {
            get { return _module; }
        }

        public MapView(MapModule module, IController controller)
        {
            _module = module;
            _controller = controller;
            _controller.Touch += ControllerOnTouch;
            digit_selectors = new List<DigitSelector>();
            floor_buttons = new List<Rectangle>();
            GenerateRectangles(xpos_floor_buttons, ypos_floor_buttons, width_floor_buttons, height_floor_buttons, margin_floor_buttons, number_floor_buttons);
            CreateDigitSelector(50, 50, 30, verdiepingen, 0);
            CreateDigitSelector(100, 50, 30, lokalen, 0);
            CreateDigitSelector(150, 50, 30, lokalen, 0);
            CreateDigitSelector(200, 50, 30, bonus, 0);
        }

        private void ControllerOnTouch(Point p)
        {
            foreach (Rectangle button in floor_buttons)
            {
                if (p.X >= xpos_floor_buttons && p.X <= xpos_floor_buttons + width_floor_buttons && p.Y >= button.Y && p.Y <= button.Y + height_floor_buttons)
                {
                    _activeFloor = floors[floor_buttons.IndexOf(button)];
                }
            }
            //if (p.X > 1550 && p.X < 1800 && p.Y > 80 && p.Y < 220)
            //    _activeFloor = Floor.T5;
            //else if (p.X > 1550 && p.X < 1800 && p.Y > 230 && p.Y < 370)
            //    _activeFloor = Floor.T4;
            //else if (p.X > 1550 && p.X < 1800 && p.Y > 380 && p.Y < 520)
            //    _activeFloor = Floor.T3;
            //else if (p.X > 1550 && p.X < 1800 && p.Y > 530 && p.Y < 670)
            //    _activeFloor = Floor.T2;
            //else if (p.X > 1550 && p.X < 1800 && p.Y > 680 && p.Y < 820)
            //    _activeFloor = Floor.T1;
            //else if (p.X > 1550 && p.X < 1800 && p.Y > 830 && p.Y < 970)
            //    _activeFloor = Floor.T0;

            int selector_index = -1;
            string change = "none";
            foreach (DigitSelector ds in digit_selectors)
            {
                foreach (ArrowButton ab in ds.buttons)
                {
                    if (ab.direction.Equals("down"))
                    {
                        if (((p.X >= ab.coords[0].X && p.X <= ab.coords[2].X) && (p.Y >= ab.coords[0].Y && p.Y <= ab.coords[0].Y + (2 * (p.X - ab.coords[0].X)))) || ((p.X >= ab.coords[2].X && p.X <= ab.coords[1].X) && (p.Y >= ab.coords[1].Y && p.Y <= ab.coords[1].Y + (2 * (ab.coords[1].X - p.X)))))
                        {
                            selector_index = digit_selectors.IndexOf(ds);
                            change = "down";
                        }
                    }
                    else if (ab.direction.Equals("up"))
                    {
                        if (((p.X >= ab.coords[0].X && p.X <= ab.coords[2].X) && (p.Y <= ab.coords[0].Y && p.Y >= ab.coords[0].Y - (2 * (p.X - ab.coords[0].X)))) || ((p.X >= ab.coords[2].X && p.X <= ab.coords[1].X) && (p.Y <= ab.coords[1].Y && p.Y >= ab.coords[1].Y - (2 * (ab.coords[1].X - p.X)))))
                        {
                            selector_index = digit_selectors.IndexOf(ds);
                            change = "up";
                        }
                    }
                }
            }
            if (selector_index != -1)
            {
                UpdateIndex(selector_index, change);
            }
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.Clear(Color.Beige);
            //button T5
            //graphics.FillRectangle(brushFloors, 1550, 80, 250, 140);
            ////button T4
            //graphics.FillRectangle(brushFloors, 1550, 230, 250, 140);
            ////button T3
            //graphics.FillRectangle(brushFloors, 1550, 380, 250, 140);
            ////button T2
            //graphics.FillRectangle(brushFloors, 1550, 530, 250, 140);
            ////button T1
            //graphics.FillRectangle(brushFloors, 1550, 680, 250, 140);
            ////button T0
            //graphics.FillRectangle(brushFloors, 1550, 830, 250, 140);
            foreach (Rectangle button in floor_buttons)
            {
                graphics.FillRectangle(brushFloors, button);
            }
            
            //switch (_activeFloor)
            //{
            //    case "T5":
            //        driehoek[0].X = 1540;
            //        driehoek[0].Y = 80;
            //        driehoek[1].X = 1540;
            //        driehoek[1].Y = 220;
            //        driehoek[2].X = 1490;
            //        driehoek[2].Y = 150;
            //        break; 
            //    case "T4":
            //        driehoek[0].X = 1540;
            //        driehoek[0].Y = 230;
            //        driehoek[1].X = 1540;
            //        driehoek[1].Y = 370;
            //        driehoek[2].X = 1490;
            //        driehoek[2].Y = 300;
            //        break;
            //    case "T3":
            //        driehoek[0].X = 1540;
            //        driehoek[0].Y = 380;
            //        driehoek[1].X = 1540;
            //        driehoek[1].Y = 520;
            //        driehoek[2].X = 1490;
            //        driehoek[2].Y = 450;
            //        break;
            //    case "T2":
            //        driehoek[0].X = 1540;
            //        driehoek[0].Y = 530;
            //        driehoek[1].X = 1540;
            //        driehoek[1].Y = 670;
            //        driehoek[2].X = 1490;
            //        driehoek[2].Y = 600;
            //        break;
            //    case "T1":
            //        driehoek[0].X = 1540;
            //        driehoek[0].Y = 680;
            //        driehoek[1].X = 1540;
            //        driehoek[1].Y = 820;
            //        driehoek[2].X = 1490;
            //        driehoek[2].Y = 750;
            //        break;
            //    case "T0":
            //        driehoek[0].X = 1540;
            //        driehoek[0].Y = 830;
            //        driehoek[1].X = 1540;
            //        driehoek[1].Y = 970;
            //        driehoek[2].X = 1490;
            //        driehoek[2].Y = 900;
            //        break;
            //}

            
            var strFont = new Font("Segoe UI", 50);
            //var rectT5 = new Rectangle(1520, 130, 300, 60);
            //var rectT4 = new Rectangle(1520, 280, 300, 60);
            //var rectT3 = new Rectangle(1520, 430, 300, 60);
            //var rectT2 = new Rectangle(1520, 580, 300, 60);
            //var rectT1 = new Rectangle(1520, 730, 300, 60);
            //var rectT0 = new Rectangle(1520, 880, 300, 60);
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            //graphics.DrawString("T5", strFont, Brushes.White, rectT5, stringFormat);
            //graphics.DrawString("T4", strFont, Brushes.White, rectT4, stringFormat);
            //graphics.DrawString("T3", strFont, Brushes.White, rectT3, stringFormat);
            //graphics.DrawString("T2", strFont, Brushes.White, rectT2, stringFormat);
            //graphics.DrawString("T1", strFont, Brushes.White, rectT1, stringFormat);
            //graphics.DrawString("T0", strFont, Brushes.White, rectT0, stringFormat);

            foreach (Rectangle button in floor_buttons) {
                if (_activeFloor.Equals("T" + (number_floor_buttons - 1 - floor_buttons.IndexOf(button)).ToString()))
                {
                    driehoek[0].X = button.X - margin_floor_buttons;
                    driehoek[0].Y = button.Y;
                    driehoek[1].X = button.X - margin_floor_buttons;
                    driehoek[1].Y = button.Y + height_floor_buttons;
                    driehoek[2].X = button.X - (5 * margin_floor_buttons);
                    driehoek[2].Y = (button.Y + button.Y + height_floor_buttons) / 2;

                }
                graphics.DrawString(floors[floor_buttons.IndexOf(button)], strFont, Brushes.White, button, stringFormat);
            }
            graphics.FillPolygon(brushActiveFloor, driehoek);
            // draw all digit selectors
            foreach (DigitSelector ds in digit_selectors)
            {
                foreach (ArrowButton ab in ds.buttons)
                {
                    graphics.FillPolygon(button_brush, ab.coords);
                }
                graphics.DrawString(ds.digitbox.values[ds.digitbox.index], text_font, text_brush, ds.digitbox.coords);
            }
        }

        private void CreateDigitSelector(int x, int y, int size, string[] values, int index)
        {
            DigitSelector ds = new DigitSelector();
            ArrowButton arrow_down = CreateArrowButton(x, y + 30, size, "down");
            ArrowButton arrow_up = CreateArrowButton(x, y, size, "up");
            ds.buttons = new List<ArrowButton>();
            ds.buttons.Add(arrow_down);
            ds.buttons.Add(arrow_up);
            ds.digitbox = CreateDigitBox(x, y, values, index);
            digit_selectors.Add(ds);
        }

        private DigitBox CreateDigitBox(int x, int y, string[] values, int index)
        {
            DigitBox db = new DigitBox();
            Point p = new Point(x, y);
            db.coords = p;
            db.values = values;
            db.index = index;
            return db;
        }

        private ArrowButton CreateArrowButton(int x, int y, int size, string direction)
        {
            ArrowButton ab = new ArrowButton();
            Point[] button = new Point[3];
            button[0].X = x;
            button[0].Y = y;
            button[1].X = x + size;
            button[1].Y = y;
            button[2].X = x + (size / 2);
            if (direction.Equals("up"))
            {
                button[2].Y = y - size;
                ab.direction = "up";
            }
            else
            {
                button[2].Y = y + size;
                ab.direction = "down";
            }
            ab.coords = button;
            return ab;
        }

        private void UpdateIndex(int selector_index, string change)
        {
            DigitSelector ds = digit_selectors[selector_index];
            DigitBox db = ds.digitbox;
            if (change.Equals("up"))
            {
                if (db.index == db.values.Length - 1)
                {
                    db.index = 0;
                }
                else
                {
                    db.index++;
                }
            }
            else
            {
                if (db.index == 0)
                {
                    db.index = db.values.Length - 1;
                }
                else
                {
                    db.index--;
                }
            }
            ds.digitbox = db;
            digit_selectors[selector_index] = ds;
        }

        public void GenerateRectangles(int xpos, int ypos, int width, int height, int margin, int number)
        {
            for (int i = 0; i < number; i++) {
                Rectangle rect = new Rectangle(xpos, ypos + (i * height) + (i * margin), width, height);
                floor_buttons.Add(rect);
            }
        }
    }
}
