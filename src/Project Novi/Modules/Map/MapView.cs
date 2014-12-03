using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private readonly MapModule _module;
      
        public IModule Module
        {
            get { return _module; }
        }

        public MapView(MapModule module)
        {
            _module = module;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.Clear(Color.Beige);
        }
    }
}
