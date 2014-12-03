using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
