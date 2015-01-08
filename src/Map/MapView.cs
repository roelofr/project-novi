using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml;
using Project_Novi.Api;
using Project_Novi.Background;
using Project_Novi.Text;

namespace Map
{
    class MapView : IView
    {
        private readonly Color _buttonColor = Color.Black;
        private readonly Color _textColor = Color.White;
        private readonly SolidBrush _markerBrush = new SolidBrush(Color.FromArgb(100, Color.Red));
        private readonly SolidBrush _arrowBrush = new SolidBrush(Color.FromArgb(100, Color.Black));
        private readonly Font _floorFont = new Font("Segoe UI", 30);
        private readonly Point[] _activeFloorArrow = new Point[3];
        private List<TouchButton> _floorButtons;       
        
        private int _xposFloorButtons;
        private int _yposFloorButtons;
        private const int WidthFloorButtons = 220;
        private const int HeightFloorButtons = 110;
        private const int MarginFloorButtons = 10;
        private const int MarginMap = 20;
        private const int XposMap = 200;
        private const int YposMap = 0;
        private Point _activePosition;
        private Rectangle _modularRectangle;
        private Stopwatch _activeTimer;
        private Stopwatch _floorTimer;
        private NumPad _floorSelectNumpad;
        private NumPadOutput _floorSelectOutput;
        private TouchButton _backspace;
        private TouchButton _tIndicator;
        private TouchButton _pIndicator;
        private int _xposNumPad;
        private int _yposNumPad;
        private const int WidthNumPad = 200;
        private const int HeightNumPad = 240;
        private const int MarkerSize = 50;
        
        private const int Scale = 100;

        private MapModule _module;
        private IController _controller;
      
        public Type ModuleType
        {
            get { return typeof(MapModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            BackgroundView = new SubBackground(controller, true);
            _controller = controller;
            
            _modularRectangle = BackgroundView.GetModuleRectangle(new Rectangle(0, 0, 1920, 1080));
        }

        public void Attach(IModule module)
        {
            var mapModule = module as MapModule;
            if (mapModule != null)
            {
                _module = mapModule;
                _controller.Touch += ControllerOnTouch;
            }
            else
                throw new ArgumentException("A MapView can only render the interface for a MapModule");

            // Display floor T0 upon attaching module
            _module.ActiveFloor = "T0";

            // Buttons for selecting floor in top right corner
            _xposFloorButtons = _modularRectangle.Width - WidthFloorButtons - MarginFloorButtons;
            _yposFloorButtons = MarginFloorButtons;
            _floorButtons = new List<TouchButton>();

            // Placing numpad between map and bottom of module, centering
            _xposNumPad = XposMap + MarginMap + ((Properties.Resources.T5x.Width*Scale/100)/2) - (WidthNumPad / 2);
            _yposNumPad = _modularRectangle.Height - (_modularRectangle.Height - (YposMap + 2 * MarginMap + Properties.Resources.T5x.Height*Scale/100)) / 2 - HeightNumPad/2;
                        
            // Create and place numpad and output for numpad
            _floorSelectNumpad = new NumPad(_xposNumPad, _yposNumPad, WidthNumPad, HeightNumPad, _module.NumPadInputs, _buttonColor, _textColor, _floorFont);
            _floorSelectOutput = new NumPadOutput(_xposNumPad, _yposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), (3 * _floorSelectNumpad.TouchButtons[0].Width) / 2, _floorSelectNumpad.TouchButtons[0].Height, 3, _floorSelectNumpad);
            _floorSelectOutput.TouchButtons[0].Xpos += _floorSelectOutput.TouchButtons[0].Width;
            _floorSelectOutput.TouchButtons[1].Xpos += 2 * _floorSelectOutput.TouchButtons[1].Width;
            _floorSelectOutput.TouchButtons[2].Xpos += 2 * _floorSelectOutput.TouchButtons[2].Width;
            _yposNumPad += _floorSelectOutput.TouchButtons[0].Height / 2;
            foreach (var button in _floorSelectOutput.TouchButtons)
            {
                button.Ypos += _floorSelectOutput.TouchButtons[0].Height/2;
            }

            foreach (var button in _floorSelectNumpad.TouchButtons)
            {
                button.Ypos += _floorSelectOutput.TouchButtons[0].Height / 2;
            }

            //Place backspace, T and . symbol
            _backspace = new TouchButton(_xposNumPad + 5 * _floorSelectOutput.TouchButtons[0].Width, _yposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), _floorSelectOutput.TouchButtons[0].Width, _floorSelectOutput.TouchButtons[0].Height, "\u2190", _buttonColor, _textColor, new Font(_floorFont.FontFamily, (int)(0.8 * (_floorSelectOutput.TouchButtons[0].Width))));
            _tIndicator = new TouchButton(_xposNumPad, _yposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), _floorSelectOutput.TouchButtons[0].Width, _floorSelectOutput.TouchButtons[0].Height, "T", _buttonColor, _textColor, new Font(_floorFont.FontFamily, (int)(0.8 * (_floorSelectOutput.TouchButtons[0].Width))));

            _tIndicator.Enabled = false;
            _pIndicator = new TouchButton(_xposNumPad + 2 * _floorSelectOutput.TouchButtons[0].Width, _yposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), _floorSelectOutput.TouchButtons[0].Width, _floorSelectOutput.TouchButtons[0].Height, ".", _buttonColor, _textColor, new Font(_floorFont.FontFamily, (int)(0.8 * (_floorSelectOutput.TouchButtons[0].Width))));
            _pIndicator.Enabled = false;

            _activeTimer = new Stopwatch();
            _floorTimer = new Stopwatch();
            _floorTimer.Start();

            // Create all buttons for selecting floors
            CreateFloorButtons(_xposFloorButtons, _yposFloorButtons, WidthFloorButtons, HeightFloorButtons, MarginFloorButtons);

            // Disable/Enable correct buttons in numpad for T0
            ButtonControl(_floorSelectNumpad, _floorSelectOutput);

            _controller.Avatar.Say(TextManager.GetText("RouteVragen"));
        }

        public void Detach()
        {
            _module = null;
        }

        private void ControllerOnTouch(Point p)
        {
            // Check if a floor button has been pressed
            foreach (var button in _floorButtons)
            {
                if (button.IsClicked(p))
                {
                    _module.ActiveFloor = _module.FloorNames[_floorButtons.IndexOf(button)];
                    _floorSelectOutput.TouchButtons.ElementAt(_floorSelectOutput.ActiveDigit).ActiveTimer.Reset();
                    _floorSelectOutput.ActiveDigit = 0;
                    _floorSelectOutput.ClearOutput(0);
                    _floorSelectOutput.TouchButtons[0].Value =
                        (_module.FloorNames.Length - 1 - _floorButtons.IndexOf(button)).ToString();
                    _floorSelectOutput.BuildOutput();
                    ButtonControl(_floorSelectNumpad, _floorSelectOutput);
                }
            }

            // Check if a button in the output field has been pressed
            foreach (var button in _floorSelectOutput.TouchButtons)
            {
                if (button.IsClicked(p))
                {
                    _floorSelectOutput.TouchButtons.ElementAt(_floorSelectOutput.ActiveDigit).ActiveTimer.Reset();
                    _floorSelectOutput.ClearOutput(_floorSelectOutput.TouchButtons.IndexOf(button) + 1);
                    _floorSelectOutput.SetActive(_floorSelectOutput.TouchButtons.IndexOf(button));
                    _floorSelectOutput.BuildOutput();
                    ButtonControl(_floorSelectNumpad, _floorSelectOutput);
                }
            }

            // Check if a button on the numpad has been pressed
            foreach (var button in _floorSelectNumpad.TouchButtons)
            {
                if (button.IsClicked(p))
                {
                    _floorSelectOutput.TouchButtons.ElementAt(_floorSelectOutput.ActiveDigit).Value = button.Value;
                    _floorSelectOutput.AddOutputDigit();
                    _floorSelectOutput.BuildOutput();
                    ButtonControl(_floorSelectNumpad, _floorSelectOutput);
                }
            }

            // Check if backspace has been pressed
            if (_backspace.IsClicked(p))
            {
                _floorSelectOutput.DeleteOutputDigit();
                _floorSelectOutput.TouchButtons.ElementAt(_floorSelectOutput.ActiveDigit).Value = "_";
                _floorSelectOutput.BuildOutput();
                ButtonControl(_floorSelectNumpad, _floorSelectOutput);
            }

            // Check if the map has been pressed
            if (p.X > XposMap + MarginMap && p.X < XposMap + MarginMap + Properties.Resources.T1x.Width*Scale / 100 && p.Y > YposMap + MarginMap && p.Y < YposMap + MarginMap + Properties.Resources.T1x.Height * Scale / 100)
            {
                var roomDict = _module.GetRoomLocationsOnFloor(Convert.ToInt32(_module.ActiveFloor[1].ToString()));              
                var roomList = roomDict.Keys;
                var closest = _module.GetClosestPoint(roomList, p, XposMap, YposMap, MarginMap);
                string room;
                roomDict.TryGetValue(new Point(closest.X, closest.Y), out room);

                _floorSelectOutput.SetActive(2);
                _floorSelectOutput.TouchButtons.ElementAt(_floorSelectOutput.ActiveDigit).ActiveTimer.Reset();

                _floorSelectOutput.TouchButtons[0].Value = room[1].ToString();
                _floorSelectOutput.TouchButtons[1].Value = room[2].ToString();
                _floorSelectOutput.TouchButtons[2].Value = "_";
                _floorSelectOutput.BuildOutput();
                ButtonControl(_floorSelectNumpad, _floorSelectOutput);

                _floorSelectOutput.TouchButtons[2].Value = room[3].ToString();

                _floorSelectOutput.BuildOutput();
                ButtonControl(_floorSelectNumpad, _floorSelectOutput);
                
                
            }
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.TranslateTransform(rectangle.X, rectangle.Y);
            _floorSelectNumpad.DrawTouchPad(graphics);
            _floorSelectOutput.DrawNumPadOutput(graphics);
            _backspace.DrawButton(graphics);
            _pIndicator.DrawButton(graphics);
            _tIndicator.DrawButton(graphics);

            graphics.FillRectangle(_arrowBrush, XposMap, YposMap, Properties.Resources.T0x.Width + 2 * MarginMap, Properties.Resources.T0x.Height + 2 * MarginMap);

            // Display correct floor map
            Bitmap bm = new Bitmap(Properties.Resources.T0x);
            switch (_module.ActiveFloor)
            {
                case "T5":
                    bm = Properties.Resources.T5x;
                    break;
                case "T4":
                    bm = Properties.Resources.T4x;
                    break;
                case "T3":
                    bm = Properties.Resources.T3x;
                    break;
                case "T2":
                    bm = Properties.Resources.T2x;
                    break;
                case "T1":
                    bm = Properties.Resources.T1x;
                    break;
                case "T0":
                    bm = Properties.Resources.T0x;
                    break;
            }
            graphics.DrawImage(bm, XposMap + MarginMap, YposMap + MarginMap, bm.Width * Scale / 100, bm.Height * Scale / 100);

            // Display all floor buttons
            foreach (var button in _floorButtons) {
                if (_module.ActiveFloor.Equals("T" + (_module.FloorNames.Length - 1 - _floorButtons.IndexOf(button)).ToString()))
                {
                    _activeFloorArrow[0].X = button.Xpos;
                    _activeFloorArrow[0].Y = button.Ypos - 2;
                    _activeFloorArrow[1].X = button.Xpos;
                    _activeFloorArrow[1].Y = button.Ypos + HeightFloorButtons + 1;
                    _activeFloorArrow[2].X = button.Xpos - (5 * MarginFloorButtons);
                    _activeFloorArrow[2].Y = (button.Ypos + button.Ypos + HeightFloorButtons) / 2;
                }                
                button.DrawButton(graphics);
            }

            // Display arrow for active floor
            if (_floorTimer.IsRunning && _floorTimer.ElapsedMilliseconds / 500 % 2 == 0)
            {
                graphics.FillPolygon(_arrowBrush, _activeFloorArrow);
            }
            
            // Display marker if a room has been selected
            if (_floorSelectOutput.Output.Length == _floorSelectOutput.Digits)
            {
                if (!_activeTimer.IsRunning)
                {
                    _activeTimer.Start();
                }
                else if (_activeTimer.ElapsedMilliseconds/500%2 == 0)
                {
                    graphics.FillEllipse(_markerBrush,
                        _activePosition.X + MarginMap - (MarkerSize / 2),
                        _activePosition.Y + MarginMap - (MarkerSize / 2), MarkerSize, MarkerSize);
                    graphics.DrawEllipse(new Pen(_markerBrush.Color, 5), _activePosition.X + MarginMap - (MarkerSize / 2),
                        _activePosition.Y + MarginMap - (MarkerSize / 2), MarkerSize, MarkerSize);
                }
            }
            else
            {
                _activeTimer.Reset();
            }

        }

        // Create all buttons for selecting floor
        public void CreateFloorButtons(int xpos, int ypos, int width, int height, int margin)
        {
            for (var i = 0; i < _module.FloorNames.Length; i++)
            {
                var touchButton = new TouchButton(xpos, ypos + (i*height) + (i*margin), width, height, _module.FloorNames[i],
                    _buttonColor, _textColor, _floorFont);
                _floorButtons.Add(touchButton);
            }
        }

 

        // Disable/Enable buttons and check output
        public void ButtonControl(NumPad numPad, NumPadOutput numPadOutput)
        {
            // Check if output entirely filled
            if (numPadOutput.Output.Length < numPadOutput.Digits)
            {
                foreach (var button in numPad.TouchButtons)
                {
                    button.Enabled = false;
                    foreach (var c in _module.GetNextDigits(numPadOutput.Output))
                    {
                        //Enable all allowed digits
                        if (button.Value.Equals(c.ToString()))
                        {                            
                            button.Enabled = true;
                        }
                    }
                }
            }

            // Check if output is filled
            if (numPadOutput.Output.Length <= numPadOutput.Digits)
            {
                foreach (var button in numPadOutput.TouchButtons)
                {
                    button.Enabled = false;
                    // Enable all filled in numbers in output field
                    if (!button.Value.Equals("_"))
                    {
                        button.Enabled = true;
                    }
                }
            }

            // Check if at least one number has been filled to allow backspace
            if (numPadOutput.Output.Length > 0)
            {
                _module.ActiveFloor = "T" + numPadOutput.Output[0];
                _backspace.Enabled = true;
            }
            else
            {
                if (numPadOutput.TouchButtons[0].Value.Equals("_"))
                {
                    _backspace.Enabled = false;
                }
                
            }

            // Check if a room has been filled in
            if (numPadOutput.Output.Length == numPadOutput.Digits)
            {
                var roomPos = _module.GetRoomLocation(numPadOutput.Output);
                _activePosition = new Point(roomPos.X * Scale / 100 + XposMap, roomPos.Y * Scale / 100 + YposMap);
                var description = _module.GetRouteDescription(numPadOutput.Output);
                _controller.Avatar.Say(description);
            }
        }
    }
}
