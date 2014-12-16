using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Api;
using System.Drawing;
using Project_Novi.Background;
using Project_Novi.Text;
using System.Net;
using System.IO;

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
            var yPos = rectangle.Y;
            var stringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var strFont = new Font("Arial", 18);
            
            

            foreach (var v in _module.tweets)
            {
                var textRect = new Rectangle(rectangle.X + 200, yPos, 600, 200);
                var tekstRect = new Rectangle(rectangle.X + 300, yPos, 600, 200);
                var imgRect = new Rectangle(rectangle.X + 130, yPos, 50, 50);
                var backgroundRect = new Rectangle(rectangle.X + 110, yPos - 20, 720, 240);

                graphics.FillRectangle(Brushes.LightBlue, backgroundRect);
                graphics.FillRectangle(Brushes.White, textRect);

                foreach (var pic in _module.pictures)
                {
                    graphics.DrawImage(pic, imgRect);
                }

                graphics.DrawString(v.ScreenName.ToString(), strFont, Brushes.Black, textRect, stringFormat);
                graphics.DrawString(v.Text, strFont, Brushes.Black, tekstRect, stringFormat);

                yPos += 300;
            }
        }
    }
}