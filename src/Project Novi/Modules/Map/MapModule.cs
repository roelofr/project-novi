using Project_Novi.Api;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
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

            sb.Append(TextManager.GetText(string.Format("MapF{0}", classroom[0])));

            //If the floor is ground floor, first floor or second floor (and we want to take the stairs)
            if (classroom[0] == '0' || classroom[0] == '1' || classroom[0] == '2')
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
            else if (classroom[0] == '4' || classroom[0] == '5' || classroom[0] == '6')
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
