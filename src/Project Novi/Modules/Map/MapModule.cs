using Project_Novi.Api;

namespace Project_Novi.Modules.Map
{
    class MapModule : IModule
    {
        private IController _controller;
        public string Name
        {
            get { return "Map"; }
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
    }
}
