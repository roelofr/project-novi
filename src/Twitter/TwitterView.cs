using System;
using System.Collections.Generic;
using Project_Novi.Api;
using System.Drawing;
using Project_Novi.Background;
using System.Xml;

namespace Twitter
{
    class TwitterView : IView
    {
        private IController _controller;
        private TwitterModule _module;
        Rectangle _usernameRect1 = new Rectangle(1000, 0, 330, 130);
        Rectangle _usernameRect2 = new Rectangle(1000, 150, 330, 130);
        Rectangle _usernameRect3 = new Rectangle(1000, 300, 330, 130);

        Rectangle _hashtagRect1 = new Rectangle(1000, 500, 330, 50);
        Rectangle _hashtagRect2 = new Rectangle(1000, 560, 330, 50);
        Rectangle _hashtagRect3 = new Rectangle(1000, 620, 330, 50);
        Rectangle _allTweetsRect = new Rectangle(1000, 700, 330, 50);

        readonly SolidBrush _brushedblack = new SolidBrush(Color.FromArgb(75, Color.Black));

        readonly StringFormat _stringFormatCenterBoth = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        readonly StringFormat _stringFormatTopLeft = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        readonly StringFormat _stringFormatBottomRight = new StringFormat
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Far
        };

        readonly StringFormat _stringFormatTopRight = new StringFormat
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Near
        };

        readonly StringFormat _stringFormatBottomLeft = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Far
        };

        readonly Font _dateFont = new Font("Arial", 14, FontStyle.Italic);
        readonly Font _textFont = new Font("Arial", 16);
        readonly Font _headFont = new Font("Arial", 18, FontStyle.Bold);

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
            if (_usernameRect1.Contains(point))
            {
                _module.TwitterAccountToDisplay = GetUsernameTwitter("username1");
            }
            if (_usernameRect2.Contains(point))
            {
                _module.TwitterAccountToDisplay = GetUsernameTwitter("username2");
            }
            if (_usernameRect3.Contains(point))
            {
                _module.TwitterAccountToDisplay = GetUsernameTwitter("username3");
            }
            if (_hashtagRect1.Contains(point))
            {
                _module.TwitterAccountToDisplay = GetUsernameTwitter("username4");
            }
            if (_hashtagRect2.Contains(point))
            {
                _module.TwitterAccountToDisplay = GetUsernameTwitter("username5");
            }
            if (_hashtagRect3.Contains(point))
            {
                _module.TwitterAccountToDisplay = GetUsernameTwitter("username6");
            }
            if (_allTweetsRect.Contains(point))
            {
                _module.TwitterAccountToDisplay = "All";
            }
        }

        public static string GetUsernameTwitter(string usernameNumber)
        {
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

        private void RenderTweets(Graphics graphics, List<Tweet> tweets)
        {
            var yPos = 20;
            foreach (var tweet in tweets)
            {
                var nameBar = new Rectangle(200, yPos, 600, 180);
                var tekstRect = new Rectangle(200, yPos + 35, 600, 100);
                var imgRect = new Rectangle(130, yPos, 50, 50);
                var backgroundRect = new Rectangle(110, yPos - 20, 720, 200);

                var date = string.Format("{0}, {1}", BackgroundUtils.GetDate(tweet.CreatedAt),
                    BackgroundUtils.GetTime(tweet.CreatedAt, false));
                
                graphics.FillRectangle(_brushedblack, backgroundRect);

                graphics.DrawImage(tweet.Image, imgRect);

                graphics.DrawString(tweet.ScreenName, _headFont, Brushes.YellowGreen, nameBar, _stringFormatTopLeft);
                graphics.DrawString(tweet.Source, _textFont, Brushes.LightGray, nameBar,
                    _stringFormatTopRight);
                graphics.DrawString(tweet.Text, _textFont, Brushes.White, tekstRect, _stringFormatTopLeft);
                graphics.DrawString(date, _dateFont, Brushes.LightGray, nameBar,
                    _stringFormatBottomRight);

                yPos += 220;
            }
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            try
            {
                if (_module.TwitterAccountToDisplay == "All")
                {
                    RenderTweets(graphics, _module.AllTweets);
                }
                if (_module.TwitterAccountToDisplay == GetUsernameTwitter("username1"))
                {
                    RenderTweets(graphics, _module.User1Tweets);
                }
                if (_module.TwitterAccountToDisplay == GetUsernameTwitter("username2"))
                {
                    RenderTweets(graphics, _module.User2Tweets);
                }
                if (_module.TwitterAccountToDisplay == GetUsernameTwitter("username3"))
                {
                    RenderTweets(graphics, _module.User3Tweets);
                }

                if (_module.TwitterAccountToDisplay == GetUsernameTwitter("username4"))
                {
                    RenderTweets(graphics, _module.Hashtag1Tweets);
                }
                if (_module.TwitterAccountToDisplay == GetUsernameTwitter("username5"))
                {
                    RenderTweets(graphics, _module.Hashtag2Tweets);
                }
                if (_module.TwitterAccountToDisplay == GetUsernameTwitter("username6"))
                {
                    RenderTweets(graphics, _module.Hashtag3Tweets);
                }

                graphics.FillRectangle(_brushedblack, _usernameRect1);
                graphics.FillRectangle(_brushedblack, _usernameRect2);
                graphics.FillRectangle(_brushedblack, _usernameRect3);
                graphics.FillRectangle(_brushedblack, _hashtagRect1);
                graphics.FillRectangle(_brushedblack, _hashtagRect2);
                graphics.FillRectangle(_brushedblack, _hashtagRect3);
                graphics.FillRectangle(_brushedblack, _allTweetsRect);

                int drawingY = 30;
                var textColor = Brushes.YellowGreen;
                var subTextColor = Brushes.LightGray;

                const int accountImageX = 1015;
                const int accountNameX = 1120;
                foreach (var account in _module.Accounts)
                {
                    graphics.DrawString(account, _headFont, textColor, accountNameX, (drawingY + 15), _stringFormatBottomLeft);
                    graphics.DrawString("@" + account, _textFont, subTextColor, accountNameX, (drawingY + 15), _stringFormatTopLeft);
                    drawingY += 150;
                }

                var img1 = new Rectangle(accountImageX, 15, 100, 100);
                var img2 = new Rectangle(accountImageX, 165, 100, 100);
                var img3 = new Rectangle(accountImageX, 315, 100, 100);
                graphics.DrawImage(_module.User1Image, img1);
                graphics.DrawImage(_module.User2Image, img2);
                graphics.DrawImage(_module.User3Image, img3);

                foreach (var hashtag in _module.Hashtags)
                {
                    graphics.DrawString(("#" + hashtag), _headFont, textColor, 1050, (drawingY + 60), _stringFormatBottomLeft);
                    drawingY += 60;
                }

                graphics.DrawString("Overzicht", _headFont, textColor, _allTweetsRect, _stringFormatCenterBoth);
            }
            catch { }
        }

    }
}
