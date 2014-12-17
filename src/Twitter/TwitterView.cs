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
            var yPos = 20;

            var stringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var stringFormat2 = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
            var stringFormat3 = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };

            var dateFont = new Font("Arial", 14, FontStyle.Italic);
            var textFont = new Font("Arial", 16);
            var headFont = new Font("Arial", 18, FontStyle.Bold);
            
            foreach (var tweet in _module.tweets)
            {
                var NameBar = new Rectangle(200, yPos, 600, 200);
                var tekstRect = new Rectangle(200, yPos + 35, 600, 200);
                var imgRect = new Rectangle(130, yPos, 50, 50);
                var backgroundRect = new Rectangle(110, yPos - 20, 720, 240);

                graphics.FillRectangle(Brushes.SteelBlue, backgroundRect);
                graphics.FillRectangle(Brushes.White, NameBar);

                foreach (var pic in _module.pictures)
                {
                    graphics.DrawImage(pic, imgRect);
                }

                graphics.DrawString(tweet.ScreenName, headFont, Brushes.YellowGreen, NameBar, stringFormat);
                graphics.DrawString("@" + _module.twitterAccountToDisplay, textFont, Brushes.Gray, NameBar, stringFormat3);
                graphics.DrawString(tweet.Text, textFont, Brushes.Black, tekstRect, stringFormat);
                graphics.DrawString(tweet.CreatedAt.ToString(), dateFont, Brushes.Black, NameBar, stringFormat2);

                yPos += 300;
            }
        }
    }
}