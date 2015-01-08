using System.Drawing;
using System.Text;
using Project_Novi.Api;
using Project_Novi.Text;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public string ActiveFloor { get; set; }
        public XmlDocument XmlDoc;

        public void Initialize(IController controller)
        {
            _controller = controller;
            XmlDoc = new XmlDocument();
            XmlDoc.Load("room_mapping.xml");
        }

        public void Start()
        {
            
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
        /// <returns></returns>
        public Point GetClosestPoint(IEnumerable<Point> pointList, Point point, int xpos, int ypos, int margin)
        {
            var closest = pointList.First();
            var closestDist =  Math.Sqrt(Math.Pow(Math.Abs((pointList.First().X + xpos + ypos) - point.X), 2) + Math.Pow(Math.Abs((pointList.First().Y + xpos + margin) - point.Y), 2));

            foreach (var _point in pointList)
            {
                var newDist =  Math.Sqrt(Math.Pow(Math.Abs((_point.X + xpos + ypos) - point.X), 2) + Math.Pow(Math.Abs((_point.Y + xpos + margin) - point.Y), 2));
                if (newDist < closestDist)
                {
                    closest = _point;
                    closestDist = newDist;
                }
            }

            return closest;
        }

    }
}
