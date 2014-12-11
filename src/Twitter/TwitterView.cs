using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Api;
using System.Drawing;
using Project_Novi.Background;
using Project_Novi.Text;

namespace Twitter
{
    class TwitterView : IView
    {
        private IController _controller;
        private TwitterModule _module;

        public Type ModuleType
        {
            get { return typeof(TwitterModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            _controller = controller;
            BackgroundView = new SubBackground(controller);
        }

        public void Attach(IModule module)
        {
            var mapModule = module as TwitterModule;
            if (mapModule != null)
            {
                _module = mapModule;
            }
            else
                throw new ArgumentException("A TwitterView can only render the interface for a TwitterModule");
        }

        public void Detach()
        {
            _module = null;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {

            // Create font and brush.

            var rectY = 300;
            var stringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };

            
           // Image twitIcon = Image.FromFile("twitter-icon.png");
            

            var strFont = new Font("Arial", 24);


            foreach (var v in _module.berichten)
            {
                var rectText = new Rectangle(700, 100, 700, rectY);
                var imgRect = new Rectangle(600, 100, 100, rectY);

                graphics.DrawString(v, strFont, Brushes.White, rectText, stringFormat);

                //graphics.DrawImage(twitIcon, imgRect);

                rectY += 300;
            }
        }
    }
}