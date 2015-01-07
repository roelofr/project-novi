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
using System.Xml;

namespace Twitter
{
    class TwitterView : IView
    {
        private IController _controller;
        private TwitterModule _module;
        Rectangle usernameRect1 = new Rectangle(1000, 0, 330, 130);
        Rectangle usernameRect2 = new Rectangle(1000, 150, 330, 130);
        Rectangle usernameRect3 = new Rectangle(1000, 300, 330, 130);
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

        private void ControllerOnTouch(Point point)
        {
            if (usernameRect1.Contains(point))
            {
                _module.twitterAccountToDisplay = GetUsernameTwitter("username1");
            }
            if (usernameRect2.Contains(point))
            {
                _module.twitterAccountToDisplay = GetUsernameTwitter("username2");
            }
            if (usernameRect3.Contains(point))
            {
                _module.twitterAccountToDisplay = GetUsernameTwitter("username3");
            }
        }
        public static string GetUsernameTwitter(string usernameNumber)
        {
            //username1
            //username2
            //username3
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");

            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/Strings/{0}/Username", usernameNumber));

            return nodeList[0].InnerText;
        }
        public void Attach(IModule module)
        {
            _controller.Touch += ControllerOnTouch;
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
            SolidBrush brushedblack = new SolidBrush(Color.FromArgb(75, Color.Black));

            var stringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var stringFormat2 = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
            var stringFormat3 = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };
            var stringFormat4 = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };

            var dateFont = new Font("Arial", 14, FontStyle.Italic);
            var textFont = new Font("Arial", 16);
            var headFont = new Font("Arial", 18, FontStyle.Bold);

            int accounts1 = 30;
          
            
            if (_module.twitterAccountToDisplay == GetUsernameTwitter("username1"))
            {
                var yPos = 20;
                foreach (var tweet in _module.tweets1)
                {
                    var NameBar = new Rectangle(200, yPos, 600, 180);
                    var tekstRect = new Rectangle(200, yPos + 35, 600, 100);
                    var imgRect = new Rectangle(130, yPos, 50, 50);
                    var backgroundRect = new Rectangle(110, yPos - 20, 720, 200);


                    graphics.FillRectangle(brushedblack, backgroundRect);

                    graphics.DrawImage(_module.usernameImage1, imgRect);

                    graphics.DrawString(tweet.ScreenName, headFont, Brushes.YellowGreen, NameBar, stringFormat);
                    graphics.DrawString("@" + _module.twitterAccountToDisplay, textFont, Brushes.LightGray, NameBar, stringFormat3);
                    graphics.DrawString(tweet.Text, textFont, Brushes.White, tekstRect, stringFormat);
                    graphics.DrawString(tweet.CreatedAt.ToString(), dateFont, Brushes.LightGray, NameBar, stringFormat2);

                    yPos += 220;
                }
            }
            if (_module.twitterAccountToDisplay == GetUsernameTwitter("username2"))
            {
                var yPos = 20;
                foreach (var tweet in _module.tweets2)
                {
                    var NameBar = new Rectangle(200, yPos, 600, 180);
                    var tekstRect = new Rectangle(200, yPos + 35, 600, 100);
                    var imgRect = new Rectangle(130, yPos, 50, 50);
                    var backgroundRect = new Rectangle(110, yPos - 20, 720, 200);


                    graphics.FillRectangle(brushedblack, backgroundRect);

                    graphics.DrawImage(_module.usernameImage2, imgRect);

                    graphics.DrawString(tweet.ScreenName, headFont, Brushes.YellowGreen, NameBar, stringFormat);
                    graphics.DrawString("@" + _module.twitterAccountToDisplay, textFont, Brushes.LightGray, NameBar, stringFormat3);
                    graphics.DrawString(tweet.Text, textFont, Brushes.White, tekstRect, stringFormat);
                    graphics.DrawString(tweet.CreatedAt.ToString(), dateFont, Brushes.LightGray, NameBar, stringFormat2);

                    yPos += 220;
                }
            }
            if (_module.twitterAccountToDisplay == GetUsernameTwitter("username3"))
            {
                var yPos = 20;
                foreach (var tweet in _module.tweets3)
                {
                    var NameBar = new Rectangle(200, yPos, 600, 180);
                    var tekstRect = new Rectangle(200, yPos + 35, 600, 100);
                    var imgRect = new Rectangle(130, yPos, 50, 50);
                    var backgroundRect = new Rectangle(110, yPos - 20, 720, 200);


                    graphics.FillRectangle(brushedblack, backgroundRect);

                   graphics.DrawImage(_module.usernameImage3, imgRect);

                    graphics.DrawString(tweet.ScreenName, headFont, Brushes.YellowGreen, NameBar, stringFormat);
                    graphics.DrawString("@" + _module.twitterAccountToDisplay, textFont, Brushes.LightGray, NameBar, stringFormat3);
                    graphics.DrawString(tweet.Text, textFont, Brushes.White, tekstRect, stringFormat);
                    graphics.DrawString(tweet.CreatedAt.ToString(), dateFont, Brushes.LightGray, NameBar, stringFormat2);

                    yPos += 220;
                }
            }
         
            graphics.FillRectangle(brushedblack, usernameRect1);
            graphics.FillRectangle(brushedblack, usernameRect2);
            graphics.FillRectangle(brushedblack, usernameRect3);
            
            foreach(string account in _module.accounts)
            {
                graphics.DrawString(account, headFont, Brushes.YellowGreen, 1120, (accounts1 + 15), stringFormat4);
                graphics.DrawString("@" + account, textFont, Brushes.LightGray, 1120, (accounts1 + 15), stringFormat);
                accounts1 += 150;
            }
                Rectangle img1 = new Rectangle(1015, 15, 100, 100);
                Rectangle img2 = new Rectangle(1015, 165, 100, 100);
                Rectangle img3 = new Rectangle(1015, 315, 100, 100);
                graphics.DrawImage(_module.usernameImage1, img1);
                graphics.DrawImage(_module.usernameImage2, img2);
                graphics.DrawImage(_module.usernameImage3, img3);
            
            }
        }
    }
