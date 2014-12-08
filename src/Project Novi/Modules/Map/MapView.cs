using System;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Background;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private MapModule _module;
        private IController _controller;
      
        public Type ModuleType
        {
            get { return typeof(MapModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            _controller = controller;
            BackgroundView = new SubBackground();
        }

        public void Attach(IModule module)
        {
            var mapModule = module as MapModule;
            if (mapModule != null)
            {
                _module = mapModule;
                _controller.Touch += ControllerOnTouch;
            }
            else
                throw new ArgumentException("A MapView can only render the interface for a MapModule");
        }

        public void Detach()
        {
            _module = null;
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
