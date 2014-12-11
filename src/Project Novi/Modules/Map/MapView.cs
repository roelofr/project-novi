using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using Project_Novi.Api;
using Project_Novi.Background;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private readonly SolidBrush _floorButtonBrush = new SolidBrush(Color.Blue);
        
        private readonly SolidBrush _floorTextBrush = new SolidBrush(Color.White);
        private readonly Font _floorFont = new Font("Segoe UI", 40);
        private readonly StringFormat _formatText = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        // Buttons for selecting floors
        private string _activeFloor = "T0";
        private readonly Point[] _activeFloorArrow = new Point[3];
        private List<TouchButton> _floorButtons;       
        private readonly string[] _floorNames = { "T5", "T4", "T3", "T2", "T1", "T0" };
        private const int XposFloorButtons = 1650;
        private const int YposFloorButtons = 250;
        private const int WidthFloorButtons = 220;
        private const int HeightFloorButtonss = 120;
        private const int MarginFloorButtons = 10;
        private int _numberFloorButtons;
        private readonly int _XposMap = 50;
        private readonly int _YposMap = 50;
        private Point _activePosition = new Point();

        // Xpos for display map, y value equals ypos of floor buttons
        private const int XposMap = 200;


        private NumPad np;
        private NumPadOutput npo;
        private TouchButton backspace;

        private MapModule _module;
        private IController _controller;
      
        public Type ModuleType
        {
            get { return typeof(MapModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            _controller = controller;
            _floorButtons = new List<TouchButton>();
            _numberFloorButtons = _floorNames.Length;
            //string[] s = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M" };
            string[] s = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};
            np = new NumPad(100, 100, 100, 150, s, Color.LightGray, Color.White, _floorFont);
            npo = new NumPadOutput(100, 50, 300, 50, 3, np);
            ButtonControl(np, npo);
            backspace = new TouchButton(400, 50, 50, 50, "←", Color.LightGray, Color.White, _floorFont);
            // Create all buttons for selecting floors
            CreateFloorButtons(XposFloorButtons, YposFloorButtons, WidthFloorButtons, HeightFloorButtonss, MarginFloorButtons, _numberFloorButtons);
            
            BackgroundView = new SubBackground(controller);
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

            _controller.Touch += ControllerOnTouch;
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
                    _activeFloor = _floorNames[_floorButtons.IndexOf(button)];
                    npo.ClearOutput();
                    npo.TouchButtons[0].Value =
                        (_numberFloorButtons - 1 - _floorButtons.IndexOf(button)).ToString();
                    npo.BuildOutput();
                    ButtonControl(np, npo);
                }
            }

            foreach (var button in npo.TouchButtons)
            {
                if (button.IsClicked(p))
                {
                    npo.SetActive(npo.TouchButtons.IndexOf(button));
                }
            }
            foreach (var button in np.TouchButtons)
            {
                if (button.IsClicked(p))
                {
                    npo.TouchButtons.ElementAt(npo.ActiveDigit).Value = button.Value;
                    npo.AddOutputDigit();
                    npo.BuildOutput();
                    ButtonControl(np, npo);
                }
            }
            if (backspace.IsClicked(p))
            {
                npo.DeleteOutputDigit();
                npo.TouchButtons.ElementAt(npo.ActiveDigit).Value = "_";
                npo.BuildOutput();
                ButtonControl(np, npo);
            }
            

        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            np.DrawTouchPad(graphics);
            npo.DrawNumPadOutput(graphics);
            backspace.DrawButton(graphics);
            // Display correct floor map
            
            switch (_activeFloor)
            {
                case "T5":
                    graphics.DrawImage(Properties.Resources.T5x, _XposMap, _YposMap);
                    break;
                case "T4":
                    graphics.DrawImage(Properties.Resources.T4x, _XposMap, _YposMap);
                    break;
                case "T3":
                    graphics.DrawImage(Properties.Resources.T3x, _XposMap, _YposMap);
                    break;
                case "T2":
                    graphics.DrawImage(Properties.Resources.T2x, _XposMap, _YposMap);
                    break;
                case "T1":
                    graphics.DrawImage(Properties.Resources.T1x, _XposMap, _YposMap);
                    break;
                case "T0":
                    graphics.DrawImage(Properties.Resources.T0x, _XposMap, _YposMap);
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
            graphics.FillPolygon(_floorButtonBrush, _activeFloorArrow);

            if (npo.Output.Length == npo.Digits)
            {
                graphics.DrawEllipse(Pens.Red, _activePosition.X, _activePosition.Y, 50, 50);
            }

        }




        // Create all buttons for selecting floor
        public void CreateFloorButtons(int xpos, int ypos, int width, int height, int margin, int number)
        {
            for (var i = 0; i < _floorNames.Length; i++)
            {
                var touchButton = new TouchButton(xpos, ypos + (i*height) + (i*margin), width, height, _floorNames[i],
                    _floorButtonBrush.Color, _floorTextBrush.Color, _floorFont);
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
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("room_mapping.xml");
            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/building/floor/room[starts-with(@id, 'T{0}')]", name));
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
            if (numPadOutput.Output.Length > 0)
            {
                _activeFloor = "T" + numPadOutput.Output[0];
            }
            if (numPadOutput.Output.Length == numPadOutput.Digits)
            {
                var roomPos = GetRoomLocation(numPadOutput.Output);
                _activePosition = new Point(roomPos.X + _XposMap, roomPos.Y + _YposMap);

            }
        }
    }
}
