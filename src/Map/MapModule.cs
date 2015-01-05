using System.Drawing;
using System.Text;
using Project_Novi.Api;
using Project_Novi.Text;

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

        public void Initialize(IController controller)
        {
            _controller = controller;
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
    }
}
