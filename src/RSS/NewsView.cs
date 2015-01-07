using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Project_Novi.Api;
using Project_Novi.Background;

namespace News
{
    internal class NewsView : IView
    {
        private IController _controller;
        private NewsModule _module;
        private readonly List<RssEntry> _visibleEntries = new List<RssEntry>();

        public Type ModuleType
        {
            get { return typeof(NewsModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            BackgroundView = new SubBackground(controller, true);
            _controller = controller;
        }

        public void Attach(IModule module)
        {
            var newsModule = module as NewsModule;
            if (newsModule != null)
            {
                _module = newsModule;
            }
            else
                throw new ArgumentException("A NewsView can only render the interface for a NewsModule");

            _module.EnableUpdate = false;

            RefreshView();

            if (_visibleEntries.Count < 1 || String.IsNullOrEmpty(_visibleEntries[0].Title))
            {
                //_controller.Avatar.Say(null);
            }
            else
            {
                _controller.Avatar.Say("Het laatste nieuws: " + _visibleEntries[0].Title);
            }
        }

        public void Detach()
        {
            _module.EnableUpdate = true;
            _module = null;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var transparentBlack = new SolidBrush(Color.FromArgb(75, Color.Black));

            var formatTL = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var formatBR = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
            var formatTR = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };
            var formatBL = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };

            var dateFont = new Font("Open Sans", 14, FontStyle.Italic);
            var baseFont = new Font("Open Sans", 16);
            var titleFont = new Font("Open Sans Semibold", 18, FontStyle.Bold);

            var yPos = 20;
            foreach (var rssEntry in _visibleEntries)
            {
                if (rssEntry.Title == null)
                    continue;

                var titleRect = new Rectangle(200, yPos, 1000, 180);
                var contentRect = new Rectangle(200, yPos + 35, 1000, 100);
                var imgRect = new Rectangle(130, yPos, 50, 50);
                var backgroundRect = new Rectangle(110, yPos - 20, 1220, 200);


                graphics.FillRectangle(transparentBlack, backgroundRect);

                if (rssEntry.Image != null)
                    graphics.DrawImage(rssEntry.Image, imgRect);

                graphics.DrawString(rssEntry.Title, titleFont, Brushes.White, titleRect, formatTL);

                if (rssEntry.Content != null)
                    graphics.DrawString(rssEntry.Content, baseFont, Brushes.White, contentRect, formatTL);

                if (!String.IsNullOrEmpty(rssEntry.Channel))
                    graphics.DrawString(rssEntry.Channel, baseFont, Brushes.LightGray, titleRect, formatBL);

                var time = String.Format("{0}, {1}", BackgroundUtils.GetDate(rssEntry.Timestamp),
                    BackgroundUtils.GetTime(rssEntry.Timestamp));
                ;
                graphics.DrawString(time, dateFont, Brushes.LightGray, titleRect, formatBR);

                yPos += 220;
            }
        }

        public void RefreshView()
        {
            _visibleEntries.Clear();

            var entryFeedCount = new Dictionary<String, Int32>();

            if (_module is NewsModule == false || _module.entries == null)
                return;

            foreach (var mod in _module.entries)
            {

                if (!entryFeedCount.ContainsKey(mod.Channel))
                    entryFeedCount.Add(mod.Channel, 0);
                entryFeedCount[mod.Channel] += 1;

                if (entryFeedCount[mod.Channel] > 2)
                    continue;

                _visibleEntries.Add(mod);
                if (_visibleEntries.Count >= 5)
                    break;
            }
        }
    }
}