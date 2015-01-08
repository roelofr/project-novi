using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Api;
using Project_Novi.Background;

namespace Vertrektijden
{
    class NSView : IView
    {
        private IController _controller;
        private NSModule _module;

        public Type ModuleType
        {
            get { return typeof(NSModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            BackgroundView = new SubBackground(controller, true);
            _controller = controller;
        }

        public void Attach(IModule module)
        {
            var newsModule = module as NSModule;
            if (newsModule != null)
            {
                _module = newsModule;
            }
            else
                throw new ArgumentException("A NewsView can only render the interface for a NewsModule");

        }

        public void Detach()
        {
            _module = null;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var font = new Font("Open Sans", 18, FontStyle.Bold);
            var align = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            
            // Not drawing anything special yet, just a "WIP" message
            graphics.DrawString("Dit systeem is nog niet beschikbaar", font, Brushes.LightGray, rectangle, align);
        }
    }
}
