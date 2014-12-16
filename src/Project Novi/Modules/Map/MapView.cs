using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml;
using Project_Novi.Api;
using Project_Novi.Background;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private readonly Color _buttonColor = Color.Black;
        private readonly Color _textColor = Color.White;
        private readonly SolidBrush _markerBrush = new SolidBrush(Color.FromArgb(100, Color.Red));
        private readonly Font _floorFont = new Font("Segoe UI", 40);
        private readonly StringFormat _formatText = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        private string _activeFloor;
        private readonly Point[] _activeFloorArrow = new Point[3];
        private List<TouchButton> _floorButtons;       
        private readonly string[] _floorNames = { "T5", "T4", "T3", "T2", "T1", "T0" };
        private readonly string[] _numPadInputs = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        private int _xposFloorButtons;
        private int _yposFloorButtons;
        private const int WidthFloorButtons = 220;
        private const int HeightFloorButtonss = 100;
        private const int MarginFloorButtons = 10;
        private int _numberFloorButtons;
        private const int XposMap = 0;
        private const int YposMap = 0;
        private Point _activePosition = new Point();
        private Rectangle _modularRectangle = new Rectangle();
        private Stopwatch _activeTimer;
        private Stopwatch _floorTimer;
        private NumPad _floorSelectNumpad;
        private NumPadOutput _floorSelectOutput;
        private TouchButton _backspace;
        private TouchButton _tIndicator;
        private TouchButton _pIndicator;
        private const int XposNumPad = 450;
        private const int YposNumPad = 500;
        private const int WidthNumPad = 240;
        private const int HeightNumPad = 280;
        private const int MarkerSize = 50;
        private XmlDocument _xmlDoc;


        private MapModule _module;
        private IController _controller;
      
        public Type ModuleType
        {
            get { return typeof(MapModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            BackgroundView = new SubBackground(controller);
            _controller = controller;

            _xmlDoc = new XmlDocument();
            _xmlDoc.Load("room_mapping.xml");
            
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

            _activeFloor = "T0";

            _xposFloorButtons = _modularRectangle.Width - WidthFloorButtons - MarginFloorButtons;
            _yposFloorButtons = MarginFloorButtons;
            _floorButtons = new List<TouchButton>();
            _numberFloorButtons = _floorNames.Length;

            _floorSelectNumpad = new NumPad(XposNumPad, YposNumPad, WidthNumPad, HeightNumPad, _numPadInputs, _buttonColor, _textColor, _floorFont);
            _floorSelectOutput = new NumPadOutput(XposNumPad, YposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), (3 * _floorSelectNumpad.TouchButtons[0].Width) / 2, _floorSelectNumpad.TouchButtons[0].Height, 3, _floorSelectNumpad);
            _floorSelectOutput.TouchButtons[0].Xpos += _floorSelectOutput.TouchButtons[0].Width;
            _floorSelectOutput.TouchButtons[1].Xpos += 2 * _floorSelectOutput.TouchButtons[1].Width;
            _floorSelectOutput.TouchButtons[2].Xpos += 2 * _floorSelectOutput.TouchButtons[2].Width;

            _backspace = new TouchButton(XposNumPad + 5 * _floorSelectOutput.TouchButtons[0].Width, YposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), _floorSelectOutput.TouchButtons[0].Width, _floorSelectOutput.TouchButtons[0].Height, "←", _buttonColor, _textColor, _floorFont);
            _tIndicator = new TouchButton(XposNumPad, YposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), _floorSelectOutput.TouchButtons[0].Width, _floorSelectOutput.TouchButtons[0].Height, "T", _buttonColor, _textColor, _floorFont);
            _tIndicator.Enabled = false;
            _pIndicator = new TouchButton(XposNumPad + 2 * _floorSelectOutput.TouchButtons[0].Width, YposNumPad - (1 * _floorSelectNumpad.TouchButtons[0].Height), _floorSelectOutput.TouchButtons[0].Width, _floorSelectOutput.TouchButtons[0].Height, ".", _buttonColor, _textColor, _floorFont);
            _pIndicator.Enabled = false;
            _activeTimer = new Stopwatch();
            _floorTimer = new Stopwatch();
            _floorTimer.Start();

            // Create all buttons for selecting floors
            CreateFloorButtons(_xposFloorButtons, _yposFloorButtons, WidthFloorButtons, HeightFloorButtonss, MarginFloorButtons, _numberFloorButtons);

            ButtonControl(_floorSelectNumpad, _floorSelectOutput);
        }

        public void Detach()
        {
            _module = null;
        }

        private void ControllerOnTouch(Point point)
        {
            var p = point;
            p.X -= _modularRectangle.X;
            p.Y -= _modularRectangle.Y;
            // Check if a floor button has been pressed
            foreach (var button in _floorButtons)
            {
                if (button.IsClicked(p))
                {
                    _activeFloor = _floorNames[_floorButtons.IndexOf(button)];
                    _floorSelectOutput.TouchButtons.ElementAt(_floorSelectOutput.ActiveDigit).ActiveTimer.Reset();
                    _floorSelectOutput.ActiveDigit = 0;
                    _floorSelectOutput.ClearOutput(0);
                    _floorSelectOutput.TouchButtons[0].Value =
                        (_numberFloorButtons - 1 - _floorButtons.IndexOf(button)).ToString();
                    _floorSelectOutput.BuildOutput();
                    ButtonControl(_floorSelectNumpad, _floorSelectOutput);
                }
            }

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
            if (_backspace.IsClicked(p))
            {
                _floorSelectOutput.DeleteOutputDigit();
                _floorSelectOutput.TouchButtons.ElementAt(_floorSelectOutput.ActiveDigit).Value = "_";
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
            // Display correct floor map
            
            switch (_activeFloor)
            {
                case "T5":
                    graphics.DrawImage(Properties.Resources.T5x, XposMap, YposMap, Properties.Resources.T5x.Width, Properties.Resources.T5x.Height);
                    graphics.DrawLine(Pens.Red, 0, 0, Properties.Resources.T5x.Width / Properties.Resources.T5x.HorizontalResolution * graphics.DpiX, 0);
                    break;
                case "T4":
                    graphics.DrawImage(Properties.Resources.T4x, XposMap, YposMap);
                    break;
                case "T3":
                    graphics.DrawImage(Properties.Resources.T3x, XposMap, YposMap);
                    break;
                case "T2":
                    graphics.DrawImage(Properties.Resources.T2x, XposMap, YposMap);
                    break;
                case "T1":
                    graphics.DrawImage(Properties.Resources.T1x, XposMap, YposMap);
                    break;
                case "T0":
                    graphics.DrawImage(Properties.Resources.T0x, XposMap, YposMap);
                    break;
            }  
            // Display all floor buttons
            foreach (var button in _floorButtons) {
                if (_activeFloor.Equals("T" + (_numberFloorButtons - 1 - _floorButtons.IndexOf(button)).ToString()))
                {
                    _activeFloorArrow[0].X = button.Xpos;
                    _activeFloorArrow[0].Y = button.Ypos;
                    _activeFloorArrow[1].X = button.Xpos;
                    _activeFloorArrow[1].Y = button.Ypos + HeightFloorButtonss;
                    _activeFloorArrow[2].X = button.Xpos - (5 * MarginFloorButtons);
                    _activeFloorArrow[2].Y = (button.Ypos + button.Ypos + HeightFloorButtonss) / 2;
                }                
                button.DrawButton(graphics);
                //graphics.DrawString(_floorNames[_floorButtons.IndexOf(button)], _floorFont, _floorTextBrush, button, _formatText);
            }

            // Display arrow for active floor
            if (_floorTimer.IsRunning && _floorTimer.ElapsedMilliseconds / 500 % 2 == 0)
            {
                graphics.FillPolygon(_markerBrush, _activeFloorArrow);
            }
            

            if (_floorSelectOutput.Output.Length == _floorSelectOutput.Digits)
            {
                if (!_activeTimer.IsRunning)
                {
                    _activeTimer.Start();
                }
                else if (_activeTimer.ElapsedMilliseconds/500%2 == 0)
                {
                    graphics.FillEllipse(_markerBrush,
                        _activePosition.X/Properties.Resources.T0x.HorizontalResolution*graphics.DpiX - (MarkerSize / 2),
                        _activePosition.Y / Properties.Resources.T0x.VerticalResolution * graphics.DpiY - (MarkerSize / 2), MarkerSize, MarkerSize);
                    graphics.DrawEllipse(new Pen(_markerBrush.Color, 5), _activePosition.X / Properties.Resources.T0x.HorizontalResolution * graphics.DpiX - (MarkerSize / 2),
                        _activePosition.Y / Properties.Resources.T0x.VerticalResolution * graphics.DpiY - (MarkerSize / 2), MarkerSize, MarkerSize);
                }
            }
            else
            {
                _activeTimer.Reset();
            }

        }




        // Create all buttons for selecting floor
        public void CreateFloorButtons(int xpos, int ypos, int width, int height, int margin, int number)
        {
            for (var i = 0; i < _floorNames.Length; i++)
            {
                var touchButton = new TouchButton(xpos, ypos + (i*height) + (i*margin), width, height, _floorNames[i],
                    _buttonColor, _textColor, _floorFont);
                _floorButtons.Add(touchButton);
            }
        }

        public char[] GetNextDigits(string start)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("room_mapping.xml");
            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/building/floor/room[starts-with(@id, 'T{0}')]", start));

            var next = from XmlNode node in nodeList
                       select node.Attributes["id"].InnerText[start.Length + 1];

            return next.ToArray();
        }

        public Point GetRoomLocation(string name)
        {
            
            var nodeList = _xmlDoc.DocumentElement.SelectNodes(String.Format("/building/floor/room[starts-with(@id, 'T{0}')]", name));
            var x = Convert.ToInt32(nodeList[0].SelectSingleNode("xPos").InnerText);
            var y = Convert.ToInt32(nodeList[0].SelectSingleNode("yPos").InnerText);

            return new Point(x, y);
        }

        public void ButtonControl(NumPad numPad, NumPadOutput numPadOutput)
        {
            if (numPadOutput.Output.Length < numPadOutput.Digits)
            {
                foreach (var button in numPad.TouchButtons)
                {
                    button.Enabled = false;
                    foreach (var c in GetNextDigits(numPadOutput.Output))
                    {
                        if (button.Value.Equals(c.ToString()))
                        {
                            button.Enabled = true;
                        }
                    }
                }
            }
            if (numPadOutput.Output.Length <= numPadOutput.Digits)
            {
                foreach (var button in numPadOutput.TouchButtons)
                {
                    button.Enabled = false;
                    if (!button.Value.Equals("_"))
                    {
                        button.Enabled = true;
                    }
                }
            }
            if (numPadOutput.Output.Length > 0)
            {
                _activeFloor = "T" + numPadOutput.Output[0];
                _backspace.Enabled = true;
            }
            else
            {
                if (numPadOutput.TouchButtons[0].Value.Equals("_"))
                {
                    _backspace.Enabled = false;
                }
                
            }

            if (numPadOutput.Output.Length == numPadOutput.Digits)
            {
                var roomPos = GetRoomLocation(numPadOutput.Output);
                _activePosition = new Point(roomPos.X + XposMap, roomPos.Y + YposMap);

            }
        }
    }
}
