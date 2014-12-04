using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private readonly MapModule _module;
        private readonly IController _controller;

        // Digit selectors for room input
        private List<DigitSelector> _digitSelectors;
        private string[] _floors = { "0", "1", "2", "3", "4", "5" };
        private string[] _rooms = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private string[] _roomAddition = { "a", "b", "c" };
        private int _sizeDigits = 50;
        private int _yposDigits = 600;
        private int _xposDigits = 50;
        private int _marginDigits = 10;

        // Brushes and fonts
        private SolidBrush _digitTextBrush = new SolidBrush(Color.Black);
        private SolidBrush _activeFloorArrowBrush = new SolidBrush(Color.Orange);
        private SolidBrush _floorButtonBrush = new SolidBrush(Color.Blue);        
        private SolidBrush _digitButtonBrush = new SolidBrush(Color.Blue);
        private SolidBrush _floorTextBrush = new SolidBrush(Color.White);
        private SolidBrush _findButtonBrush = new SolidBrush(Color.Orange);
        private SolidBrush _findTextBrush = new SolidBrush(Color.White);
        private Font _digitFont = new Font("Segoe UI", 20);
        private Font _floorFont = new Font("Segoe UI", 40);
        private StringFormat _formatText = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        // Buttons for selecting floors
        private string _activeFloor = "T0";
        private Point[] _activeFloorArrow = new Point[3];
        private List<Rectangle> _floorButtons;       
        private string[] _floorNames = { "T5", "T4", "T3", "T2", "T1", "T0" };
        private int _xposFloorButtons = 1650;
        private int _yposFloorButtons = 250;
        private int _widthFloorButtons = 220;
        private int _heightFloorButtonss = 120;
        private int _marginFloorButtons = 10;
        private int _numberFloorButtons;

        // Xpos for display map, y value equals ypos of floor buttons
        private int _xposMap = 200;

        private int _xposFindButton = 100;
        private int _yposFindButton = 800;
        private int _sizeFindButton = 150;
        
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
            _digitSelectors = new List<DigitSelector>();
            _floorButtons = new List<Rectangle>();
            _numberFloorButtons = _floorNames.Length;

            // Create all buttons for selecting floors
            CreateFloorButtons(_xposFloorButtons, _yposFloorButtons, _widthFloorButtons, _heightFloorButtonss, _marginFloorButtons, _numberFloorButtons);

            // Create all selectors for inputting room
            CreateDigitSelector(_xposDigits, _yposDigits, _sizeDigits, _floors, 0);
            CreateDigitSelector(_xposDigits + (1 * _sizeDigits) + (1 * _marginDigits), _yposDigits, _sizeDigits, _rooms, 0);
            CreateDigitSelector(_xposDigits + (2 * _sizeDigits) + (2 * _marginDigits), _yposDigits, _sizeDigits, _rooms, 0);
            CreateDigitSelector(_xposDigits + (3 * _sizeDigits) + (3 * _marginDigits), _yposDigits, _sizeDigits, _roomAddition, 0);
        }

        private void ControllerOnTouch(Point p)
        {
            // Check if a floor button has been pressed
            foreach (Rectangle button in _floorButtons)
            {
                if (p.X >= _xposFloorButtons && p.X <= _xposFloorButtons + _widthFloorButtons && p.Y >= button.Y && p.Y <= button.Y + _heightFloorButtonss)
                {
                    _activeFloor = _floorNames[_floorButtons.IndexOf(button)];
                    FloorSelect(_floorButtons.IndexOf(button));
                }
            }

            // Check if a digit selector has been pressed
            int selector_index = -1;
            string change = "none";
            foreach (DigitSelector ds in _digitSelectors)
            {
                foreach (ArrowButton ab in ds.buttons)
                {
                    if (ab.direction.Equals("down"))
                    {
                        if (((p.X >= ab.coords[0].X && p.X <= ab.coords[2].X) && (p.Y >= ab.coords[0].Y && p.Y <= ab.coords[0].Y + (2 * (p.X - ab.coords[0].X)))) || ((p.X >= ab.coords[2].X && p.X <= ab.coords[1].X) && (p.Y >= ab.coords[1].Y && p.Y <= ab.coords[1].Y + (2 * (ab.coords[1].X - p.X)))))
                        {
                            selector_index = _digitSelectors.IndexOf(ds);
                            change = "down";
                        }
                    }
                    else if (ab.direction.Equals("up"))
                    {
                        if (((p.X >= ab.coords[0].X && p.X <= ab.coords[2].X) && (p.Y <= ab.coords[0].Y && p.Y >= ab.coords[0].Y - (2 * (p.X - ab.coords[0].X)))) || ((p.X >= ab.coords[2].X && p.X <= ab.coords[1].X) && (p.Y <= ab.coords[1].Y && p.Y >= ab.coords[1].Y - (2 * (ab.coords[1].X - p.X)))))
                        {
                            selector_index = _digitSelectors.IndexOf(ds);
                            change = "up";
                        }
                    }
                }
            }

            // Update digits if one has been pressed
            if (selector_index != -1)
            {
                UpdateIndex(selector_index, change);
            }

            if (p.X >= _xposFindButton && p.X <= _xposFindButton + _sizeFindButton && p.Y >= _yposFindButton && p.Y <= (_yposFindButton + (_sizeFindButton / 2)))
            {
                MessageBox.Show("TEST");
            }
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            // Display correct floor map
            switch(_activeFloor)
            {
                case "T5":
                    graphics.DrawImage(Properties.Resources.T5x, _xposMap, _yposFloorButtons);
                    break;
                case "T4":
                    graphics.DrawImage(Properties.Resources.T4x, _xposMap, _yposFloorButtons);
                    break;
                case "T3":
                    graphics.DrawImage(Properties.Resources.T3x, _xposMap, _yposFloorButtons);
                    break;
                case "T2":
                    graphics.DrawImage(Properties.Resources.T2x, _xposMap, _yposFloorButtons);
                    break;
                case "T1":
                    graphics.DrawImage(Properties.Resources.T1x, _xposMap, _yposFloorButtons);
                    break;
                case "T0":
                    graphics.DrawImage(Properties.Resources.T0x, _xposMap, _yposFloorButtons);
                    break;
            }            

            // Display all floor buttons
            foreach (Rectangle button in _floorButtons) {
                if (_activeFloor.Equals("T" + (_numberFloorButtons - 1 - _floorButtons.IndexOf(button)).ToString()))
                {
                    _activeFloorArrow[0].X = button.X - _marginFloorButtons;
                    _activeFloorArrow[0].Y = button.Y;
                    _activeFloorArrow[1].X = button.X - _marginFloorButtons;
                    _activeFloorArrow[1].Y = button.Y + _heightFloorButtonss;
                    _activeFloorArrow[2].X = button.X - (5 * _marginFloorButtons);
                    _activeFloorArrow[2].Y = (button.Y + button.Y + _heightFloorButtonss) / 2;
                }                
                graphics.FillRectangle(_floorButtonBrush, button);
                graphics.DrawString(_floorNames[_floorButtons.IndexOf(button)], _floorFont, _floorTextBrush, button, _formatText);
            }

            // Display arrow for active floor
            graphics.FillPolygon(_activeFloorArrowBrush, _activeFloorArrow);

            // Display all digit selectors
            foreach (DigitSelector ds in _digitSelectors)
            {
                foreach (ArrowButton ab in ds.buttons)
                {
                    graphics.FillPolygon(_digitButtonBrush, ab.coords);
                }
                Rectangle digit_box = new Rectangle(ds.digitbox.coords.X, ds.digitbox.coords.Y, _sizeDigits, _sizeDigits);
                graphics.DrawString(ds.digitbox.values[ds.digitbox.index], _digitFont, _digitTextBrush, digit_box, _formatText);
            }

            Rectangle find_box = new Rectangle(_xposFindButton, _yposFindButton, _sizeFindButton, _sizeFindButton / 2);
            graphics.FillRectangle(_findButtonBrush, _xposFindButton, _yposFindButton, _sizeFindButton, _sizeFindButton / 2);
            graphics.DrawString("Vind", _floorFont, _findTextBrush, find_box, _formatText);
        }

        // Add a new digit selector to list
        private void CreateDigitSelector(int x, int y, int size, string[] values, int index)
        {
            DigitSelector ds = new DigitSelector();
            ArrowButton arrow_down = CreateArrowButton(x, y, size, "down");
            ArrowButton arrow_up = CreateArrowButton(x, y, size, "up");
            ds.buttons = new List<ArrowButton>();
            ds.buttons.Add(arrow_down);
            ds.buttons.Add(arrow_up);
            ds.digitbox = CreateDigitBox(x, y, values, index);
            _digitSelectors.Add(ds);
        }

        // Return a DigitBox for DigitSelector being created
        private DigitBox CreateDigitBox(int x, int y, string[] values, int index)
        {
            DigitBox db = new DigitBox();
            Point p = new Point(x, y);
            db.coords = p;
            db.values = values;
            db.index = index;
            return db;
        }

        // Return an ArrowButton for DigitSelector being created
        private ArrowButton CreateArrowButton(int x, int y, int size, string direction)
        {
            ArrowButton ab = new ArrowButton();
            Point[] button = new Point[3];
            button[0].X = x;            
            button[1].X = x + size;            
            button[2].X = x + (size / 2);

            if (direction.Equals("up"))
            {
                button[0].Y = y;
                button[1].Y = y;
                button[2].Y = y - size;
                ab.direction = "up";
            }
            else
            {
                button[0].Y = y + size;
                button[1].Y = y + size;
                button[2].Y = y + (2 * size);
                ab.direction = "down";
            }
            ab.coords = button;
            return ab;
        }

        // Update Digitbox if an ArrowButton has been pressed
        private void UpdateIndex(int selector_index, string change)
        {
            DigitSelector ds = _digitSelectors[selector_index];
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
            _digitSelectors[selector_index] = ds;
        }

        // Update first DigitSelector if another floor has been selected
        private void FloorSelect(int floor)
        {
            DigitSelector ds = _digitSelectors[0];
            DigitBox db = ds.digitbox;
            db.index = _floors.Length - 1 - floor;
            ds.digitbox = db;
            _digitSelectors[0] = ds;
        }

        // Create all buttons for selecting floor
        public void CreateFloorButtons(int xpos, int ypos, int width, int height, int margin, int number)
        {
            for (int i = 0; i < number; i++) {
                Rectangle rect = new Rectangle(xpos, ypos + (i * height) + (i * margin), width, height);
                _floorButtons.Add(rect);
            }
        }
    }
}
