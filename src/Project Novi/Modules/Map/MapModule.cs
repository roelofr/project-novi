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

        public string GetRouteDescription(string classroom)
        {
            var numbers = classroom.ToCharArray();
            var sb = new StringBuilder();

            sb.Append(TextManager.GetText(string.Format("MapF{0}", numbers[0])));
            sb.Append(TextManager.GetText(string.Format("MapC{0}", numbers[1])));

            return sb.ToString();
        }
    }
}
