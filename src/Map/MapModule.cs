using System.Drawing;
using System.Text;
using Project_Novi.Api;
using Project_Novi.Text;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Project_Novi.Background;

namespace Map
{
    class MapModule : IModule
    {
        private IController _controller;
        public string Name
        {
            get { return "Map"; }
        }

        public Bitmap Icon
        {
            get { return Properties.Resources.Icon; }
        }

        public string DisplayName
        {
            get { return "Kaart"; }
        }

        public bool Rotatable
        {
            get { return false; }
        }

        public readonly string[] FloorNames = { "T5", "T4", "T3", "T2", "T1", "T0" };
        public readonly string[] NumPadInputs = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        public string ActiveFloor {get; set; }
        public XmlDocument XmlDoc;
        public Point ActivePoint { get; set; }

        private Rectangle _modularRectangle;
        public Stopwatch ActiveTimer { get; set; }
        public Stopwatch FloorTimer { get; set; }

        ///
        public List<TouchButton> FloorButtons { get; set; }
        public NumPad FloorSelectNumpad;
        public NumPadOutput FloorSelectOutput;
        public TouchButton Backspace;
        public TouchButton BuildingIndicator;
        public TouchButton PointIndicator;

        public enum ButtonType
        {
            FloorButton,
            OutputButton,
            NumpadButton,
            Backspace
        };

        private readonly Color _buttonColor = Color.Black;
        private readonly Color _textColor = Color.White;
        private readonly Font _floorFont = new Font("Segoe UI", 30);

        private int _xposFloorButtons;
        private int _yposFloorButtons;
        public int WidthFloorButtons = 220;
        public int HeightFloorButtons = 110;
        public int MarginFloorButtons = 10;
        public int MarginMap = 20;
        public int XposMap = 200;
        public int YposMap = 0;

        private int _xposNumPad;
        private int _yposNumPad;
        private const int WidthNumPad = 200;
        private const int HeightNumPad = 240;
        public int Scale = 100;

        public void Initialize(IController controller)
        {
            _controller = controller;

            //Load all rooms
            XmlDoc = new XmlDocument();
            XmlDoc.Load("room_mapping.xml");

            ActiveFloor = "T0";

            var sb = new SubBackground(_controller);
            _modularRectangle = sb.GetModuleRectangle(new Rectangle(0, 0, 1920, 1080));

            //Determine positions and create all buttons for selecting floors
            _xposFloorButtons = _modularRectangle.Width - WidthFloorButtons - MarginFloorButtons;
            _yposFloorButtons = MarginFloorButtons;
            FloorButtons = new List<TouchButton>();
            CreateFloorButtons(_xposFloorButtons, _yposFloorButtons, WidthFloorButtons, HeightFloorButtons, MarginFloorButtons);

            //Determine position and create numpad
            _xposNumPad = XposMap + MarginMap + ((GetDisplayMap().Width * Scale / 100) / 2) - (WidthNumPad / 2);
            _yposNumPad = _modularRectangle.Height - (_modularRectangle.Height - (YposMap + 2 * MarginMap + Properties.Resources.T5x.Height * Scale / 100)) / 2 - HeightNumPad / 2;
            FloorSelectNumpad = new NumPad(_xposNumPad, _yposNumPad, WidthNumPad, HeightNumPad, NumPadInputs, _buttonColor, _textColor, _floorFont);

            //Create output for numpad
            FloorSelectOutput = new NumPadOutput(_xposNumPad, _yposNumPad - (1 * FloorSelectNumpad.TouchButtons[0].Height), (3 * FloorSelectNumpad.TouchButtons[0].Width) / 2, FloorSelectNumpad.TouchButtons[0].Height, 3, FloorSelectNumpad);
            
            //Move outputfields to fit in T, . and backspace symbol
            FloorSelectOutput.TouchButtons[0].Xpos += FloorSelectOutput.TouchButtons[0].Width;
            FloorSelectOutput.TouchButtons[1].Xpos += 2 * FloorSelectOutput.TouchButtons[1].Width;
            FloorSelectOutput.TouchButtons[2].Xpos += 2 * FloorSelectOutput.TouchButtons[2].Width;
            _yposNumPad += FloorSelectOutput.TouchButtons[0].Height / 2;
            foreach (var button in FloorSelectOutput.TouchButtons)
            {
                button.Ypos += FloorSelectOutput.TouchButtons[0].Height / 2;
            }

            foreach (var button in FloorSelectNumpad.TouchButtons)
            {
                button.Ypos += FloorSelectOutput.TouchButtons[0].Height / 2;
            }

            //Place backspace, T and . symbol
            Backspace = new TouchButton(_xposNumPad + 5 * FloorSelectOutput.TouchButtons[0].Width, _yposNumPad - (1 * FloorSelectNumpad.TouchButtons[0].Height), FloorSelectOutput.TouchButtons[0].Width, FloorSelectOutput.TouchButtons[0].Height, "\u2190", _buttonColor, _textColor, new Font(_floorFont.FontFamily, (int)(0.8 * (FloorSelectOutput.TouchButtons[0].Width))));
            BuildingIndicator = new TouchButton(_xposNumPad, _yposNumPad - (1 * FloorSelectNumpad.TouchButtons[0].Height), FloorSelectOutput.TouchButtons[0].Width, FloorSelectOutput.TouchButtons[0].Height, "T", _buttonColor, _textColor, new Font(_floorFont.FontFamily, (int)(0.8 * (FloorSelectOutput.TouchButtons[0].Width))));
            BuildingIndicator.Enabled = false;
            PointIndicator = new TouchButton(_xposNumPad + 2 * FloorSelectOutput.TouchButtons[0].Width, _yposNumPad - (1 * FloorSelectNumpad.TouchButtons[0].Height), FloorSelectOutput.TouchButtons[0].Width, FloorSelectOutput.TouchButtons[0].Height, ".", _buttonColor, _textColor, new Font(_floorFont.FontFamily, (int)(0.8 * (FloorSelectOutput.TouchButtons[0].Width))));
            PointIndicator.Enabled = false;
        }

        public void Start()
        {
            ActiveTimer = new Stopwatch();
            FloorTimer = new Stopwatch();
            FloorTimer.Start();

            //Set T0 as default display
            ActiveFloor = "T0";

            //Clear outputfield and update
            FloorSelectOutput.DeleteOutput();
            Update();
        }

        public void Stop()
        {
           
        }

        
        /// <summary>
        /// Returns a textual route description based on the classroom code given
        /// </summary>
        /// <param name="classroom">Classroom code, consists of 3 numbers</param>
        /// <returns></returns>
        public string GetRouteDescription(string classroom)
        {
            var sb = new StringBuilder();

            sb.Append(TextManager.GetText(string.Format("MapF{0}", classroom[0])));

            if (classroom[0] == '0')
            {
                switch (classroom[1])
                {
                    case '0':
                    case '1':
                    case '2':
                        sb.Append(TextManager.GetText("MapBeganeGrondLinks"));
                        break;
                    case '3':
                        //If the classroom is in the middle of the building
                        sb.Append(TextManager.GetText("MapBeganeGrondMidden"));
                        break;
                    case '4':
                    case '5':
                    case '6':
                        //If the classroom is in the left half of the building
                        sb.Append(TextManager.GetText("MapBeganeGrondRechts"));
                        break;
                }
            }
            //If the floor is ground floor, first floor or second floor (and we want to take the stairs)
            else if (classroom[0] == '0' || classroom[0] == '1' || classroom[0] == '2')
            {
                switch (classroom[1])
                {
                    case '0':
                    case '1':
                    case '2':
                        //If the classroom is in the right half of the building
                        sb.Append(TextManager.GetText("MapTrapRechts"));
                        break;
                    case '3':
                        //If the classroom is in the middle of the building
                        sb.Append(TextManager.GetText("MapMidden"));
                        break;
                    case '4':
                    case '5':
                    case '6':
                        //If the classroom is in the left half of the building
                        sb.Append(TextManager.GetText("MapTrapLinks"));
                        break;
                }
            }
            //Else if the floor is third floor, fourth floor or fifth floor (and we want to take the elevator)
            else if (classroom[0] == '3' || classroom[0] == '4' || classroom[0] == '5')
            {
                switch (classroom[1])
                {
                    case '0':
                    case '1':
                    case '2':
                        //If the classroom is in the left half of the building
                        sb.Append(TextManager.GetText("MapLiftLinks"));
                        break;
                    case '3':
                        //If the classroom is in the middle of the building
                        sb.Append(TextManager.GetText("MapMidden"));
                        break;
                    case '4':
                    case '5':
                    case '6':
                        //If the classroom is in the right half of the building
                        sb.Append(TextManager.GetText("MapLiftRechts"));
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Return coordinates of given room
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Point GetRoomLocation(string name)
        {
            var nodeList = XmlDoc.DocumentElement.SelectNodes(String.Format("/building/floor/room[starts-with(@id, 'T{0}')]", name));
            var x = Convert.ToInt32(nodeList[0].SelectSingleNode("xPos").InnerText);
            var y = Convert.ToInt32(nodeList[0].SelectSingleNode("yPos").InnerText);

            return new Point(x, y);
        }

        /// <summary>
        /// Returns all the classroomcodes and their locations for a given floor
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public Dictionary<Point, string> GetRoomLocationsOnFloor(int floor)
        {
            var nodeList = XmlDoc.DocumentElement.SelectNodes(String.Format("/building/floor[@id='{0}']/room", floor));

            return nodeList.Cast<XmlNode>().ToDictionary(room => new Point(Convert.ToInt32(room.SelectSingleNode("xPos").InnerText), Convert.ToInt32(room.SelectSingleNode("yPos").InnerText)), room => room.Attributes[0].InnerXml);
        }

        /// <summary>
        /// Returns all available next digits for current room input
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public char[] GetNextDigits(string start)
        {
            var nodeList = XmlDoc.DocumentElement.SelectNodes(String.Format("/building/floor/room[starts-with(@id, 'T{0}')]", start));

            var next = from XmlNode node in nodeList
                       select node.Attributes["id"].InnerText[start.Length + 1];

            return next.ToArray();
        }

        /// <summary>
        /// Calculates which point from a given list is closest to the given point
        /// </summary>
        /// <param name="pointList">List of points to compare to given point</param>
        /// <param name="point"></param>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public Point GetClosestPoint(IEnumerable<Point> pointList, Point point, int xpos, int ypos, int margin)
        {
            var closest = pointList.First();
            var closestDist =  Math.Sqrt(Math.Pow(Math.Abs((pointList.First().X + xpos + margin) - point.X), 2) + Math.Pow(Math.Abs((pointList.First().Y + ypos + margin) - point.Y), 2));

            foreach (var room in pointList)
            {
                var newDist =  Math.Sqrt(Math.Pow(Math.Abs((room.X + xpos + margin) - point.X), 2) + Math.Pow(Math.Abs((room.Y + ypos + margin) - point.Y), 2));
                if (newDist < closestDist)
                {
                    closest = room;
                    closestDist = newDist;
                }
            }

            return closest;
        }

        /// <summary>
        /// Returns map of currently active floor
        /// </summary>
        /// <returns></returns>
        public Bitmap GetDisplayMap()
        {
            switch (ActiveFloor)
            {
                case "T5":
                    return Properties.Resources.T5x;
                case "T4":
                    return Properties.Resources.T4x;
                case "T3":
                    return Properties.Resources.T3x;
                case "T2":
                    return Properties.Resources.T2x;
                case "T1":
                    return Properties.Resources.T1x;
                case "T0":
                    return Properties.Resources.T0x;
                default:
                    return Properties.Resources.T0x;
            }
        }

        /// <summary>
        /// Update for all buttons whether they should be enabled or not and if a room has been selected
        /// </summary>
        public void ButtonControl()
        {
            // Check if output entirely filled
            if (FloorSelectOutput.Output.Length < FloorSelectOutput.Digits)
            {
                foreach (var button in FloorSelectNumpad.TouchButtons)
                {
                    button.Enabled = false;
                    foreach (var c in GetNextDigits(FloorSelectOutput.Output))
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
            if (FloorSelectOutput.Output.Length <= FloorSelectOutput.Digits)
            {
                foreach (var button in FloorSelectOutput.TouchButtons)
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
            if (FloorSelectOutput.Output.Length > 0)
            {
                ActiveFloor = "T" + FloorSelectOutput.Output[0];
                Backspace.Enabled = true;
            }

            else
            {
                if (FloorSelectOutput.TouchButtons[0].Value.Equals("_"))
                {
                    Backspace.Enabled = false;
                }

            }

            // Check if a room has been filled in
            if (FloorSelectOutput.Output.Length == FloorSelectOutput.Digits)
            {
                var roomPos = GetRoomLocation(FloorSelectOutput.Output);
                ActivePoint = new Point(roomPos.X * Scale / 100 + XposMap, roomPos.Y * Scale / 100 + YposMap);
                var description = GetRouteDescription(FloorSelectOutput.Output);
                _controller.Avatar.Say(description);
            }
        }

        /// <summary>
        /// Create all buttons for selecting floor
        /// </summary>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="margin"></param>
        public void CreateFloorButtons(int xpos, int ypos, int width, int height, int margin)
        {
            for (var i = 0; i < FloorNames.Length; i++)
            {
                var touchButton = new TouchButton(xpos, ypos + (i * height) + (i * margin), width, height, FloorNames[i],
                    _buttonColor, _textColor, _floorFont);
                FloorButtons.Add(touchButton);
            }
        }

        /// <summary>
        /// Update outputfield based on clicked button
        /// </summary>
        /// <param name="buttonType"></param>
        /// <param name="button"></param>
        public void HandleTouchButton(ButtonType buttonType, TouchButton button)
        {
            switch (buttonType)
            {
                case ButtonType.FloorButton:
                    ActiveFloor = FloorNames[FloorButtons.IndexOf(button)];
                    FloorSelectOutput.TouchButtons.ElementAt(FloorSelectOutput.ActiveDigit).ActiveTimer.Reset();
                    FloorSelectOutput.ActiveDigit = 0;
                    FloorSelectOutput.ClearOutput(0);
                    FloorSelectOutput.TouchButtons[0].Value =
                        (FloorNames.Length - 1 - FloorButtons.IndexOf(button)).ToString();
                    break;
                case ButtonType.OutputButton:
                    FloorSelectOutput.TouchButtons.ElementAt(FloorSelectOutput.ActiveDigit).ActiveTimer.Reset();
                    FloorSelectOutput.ClearOutput(FloorSelectOutput.TouchButtons.IndexOf(button) + 1);
                    FloorSelectOutput.SetActive(FloorSelectOutput.TouchButtons.IndexOf(button));
                    break;
                case ButtonType.NumpadButton:
                    FloorSelectOutput.TouchButtons.ElementAt(FloorSelectOutput.ActiveDigit).Value = button.Value;
                    FloorSelectOutput.AddOutputDigit();
                    break;
                case ButtonType.Backspace:
                    FloorSelectOutput.DeleteOutputDigit();
                    FloorSelectOutput.TouchButtons.ElementAt(FloorSelectOutput.ActiveDigit).Value = "_";
                    break;
            }
            Update();
        }

        /// <summary>
        /// Update outputfield based on clicked position on map
        /// </summary>
        /// <param name="p"></param>
        public void HandleTouchMap(Point p)
        {
            var roomDict = GetRoomLocationsOnFloor(Convert.ToInt32(ActiveFloor[1].ToString()));
            var roomList = roomDict.Keys;
            var closest = GetClosestPoint(roomList, p, XposMap, YposMap, MarginMap);
            string room;
            roomDict.TryGetValue(new Point(closest.X, closest.Y), out room);

            FloorSelectOutput.SetActive(2);
            FloorSelectOutput.TouchButtons.ElementAt(FloorSelectOutput.ActiveDigit).ActiveTimer.Reset();

            FloorSelectOutput.TouchButtons[0].Value = room[1].ToString();
            FloorSelectOutput.TouchButtons[1].Value = room[2].ToString();
            FloorSelectOutput.TouchButtons[2].Value = "_";

            Update();

            FloorSelectOutput.TouchButtons[2].Value = room[3].ToString();

            Update();

        }

        /// <summary>
        /// Update both outputfield and all buttons
        /// </summary>
        public void Update()
        {
            FloorSelectOutput.BuildOutput();
            ButtonControl();
        }
    }
}
