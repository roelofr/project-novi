using Project_Novi.Api;
﻿using System;
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

            //If the floor is ground floor, first floor or second floor (and we want to take the stairs)
            if (classroom[0].ToString().Equals("0") || classroom[0].ToString().Equals("1") || classroom[0].ToString().Equals("2"))
            {
                //If the classroom is in the right half of the building
                if (classroom[1].ToString().Equals("0") || classroom[1].ToString().Equals("1") || classroom[1].ToString().Equals("2"))
                {
                    sb.Append(TextManager.GetText("MapTrapRechts"));
                }
                //Else if the classroom is in the middle of the building
                else if (classroom[1].ToString().Equals("3"))
                {
                    sb.Append(TextManager.GetText("MapTrapMidden"));
                }
                //Else if the classroom is in the left half of the building
                else if (classroom[1].ToString().Equals("4") || classroom[1].ToString().Equals("5") || classroom[1].ToString().Equals("6"))
                {
                    sb.Append(TextManager.GetText("MapTrapLinks"));
                }
            }
            //Else if the floor is third floor, fourth floor or fifth floor (and we want to take the elevator)
            else if (classroom[0].ToString().Equals("3") || classroom[0].ToString().Equals("4") || classroom[0].ToString().Equals("5"))
            {
                //If the classroom is in the right half of the building
                if (classroom[1].ToString().Equals("0") || classroom[1].ToString().Equals("1") || classroom[1].ToString().Equals("2"))
                {
                    sb.Append(TextManager.GetText("MapLiftLinks"));
                }
                //Else if the classroom is in the middle of the building
                else if (classroom[1].ToString().Equals("3"))
                {
                    sb.Append(TextManager.GetText("MapLiftMidden"));
                }
                //Else if the classroom is in the left half of the building
                else if (classroom[1].ToString().Equals("4") || classroom[1].ToString().Equals("5") || classroom[1].ToString().Equals("6"))
                {
                    sb.Append(TextManager.GetText("MapLiftRechts"));
                }
            }

            return sb.ToString();
        }
    }
}
