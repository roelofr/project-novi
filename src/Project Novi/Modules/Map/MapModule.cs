using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Text;

namespace Project_Novi.Modules.Map
{
    class MapModule : IModule
    {
        private IController _controller;
        public string Name
        {
            get { return "Map"; }
        }

        public string WelcomeText
        {
            get { throw new NotImplementedException(); }
        }

        public MapModule(IController controller)
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
            var numbers = classroom.ToCharArray();
            var sb = new StringBuilder();

            //If the floor is ground floor, first floor or second floor (and we want to take the stairs)
            if (numbers[0].Equals("0") || numbers[0].Equals("1") || numbers[0].Equals("2"))
            {
                //If the classroom is in the right half of the building
                if (numbers[1].Equals("0") || numbers[1].Equals("1") || numbers[1].Equals("2"))
                {
                    sb.Append(TextManager.GetText("MapTrapRechts"));
                }
                //Else if the classroom is in the middle of the building
                else if (numbers[1].Equals("3"))
                {
                    sb.Append(TextManager.GetText("MapTrapMidden"));
                }
                //Else if the classroom is in the left half of the building
                else if (numbers[1].Equals("4") || numbers[1].Equals("5") || numbers[1].Equals("6"))
                {
                    sb.Append(TextManager.GetText("MapTrapLinks"));
                }
            }
            //Else if the floor is third floor, fourth floor or fifth floor (and we want to take the elevator)
            else if (numbers[0].Equals("3") || numbers[0].Equals("4") || numbers[0].Equals("5"))
            {
                //If the classroom is in the right half of the building
                if (numbers[1].Equals("0") || numbers[1].Equals("1") || numbers[1].Equals("2"))
                {
                    sb.Append(TextManager.GetText("MapLiftLinks"));
                }
                //Else if the classroom is in the middle of the building
                else if (numbers[1].Equals("3"))
                {
                    sb.Append(TextManager.GetText("MapLiftMidden"));
                }
                //Else if the classroom is in the left half of the building
                else if (numbers[1].Equals("4") || numbers[1].Equals("5") || numbers[1].Equals("6"))
                {
                    sb.Append(TextManager.GetText("MapLiftRechts"));
                }
            }

            return sb.ToString();
        }
    }
}
