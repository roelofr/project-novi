using System.Drawing;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private readonly MapModule _module;
        private readonly IController _controller;
      
        public IModule Module
        {
            get { return _module; }
        }

        public MapView(MapModule module, IController controller)
        {
            _module = module;
            _controller = controller;
            _controller.Touch += ControllerOnTouch;
        }

        private void ControllerOnTouch(Point p)
        {
            
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            //graphics.Clear(Color.Beige);
        }
    }
}
