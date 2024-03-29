﻿using System;
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

        // Fonts and alignment
        private readonly SolidBrush _transparentBlack = new SolidBrush(Color.FromArgb(75, Color.Black));

        private readonly StringFormat _topLeftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
        private readonly StringFormat _topCenterFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
        //private readonly StringFormat _topRightFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near }; // Not used
        private readonly StringFormat _bottomLeftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };
        private readonly StringFormat _bottomRightFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

        private readonly Font _dateFont = new Font("Open Sans", 14, FontStyle.Italic);
        private readonly Font _baseFont = new Font("Open Sans", 16);
        private readonly Font _titleFont = new Font("Open Sans Semibold", 18, FontStyle.Bold);

        private DateTime _visibleTime;
        private DateTime _lastNoDataTime;
        private bool HasSpoken = false;

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

            _visibleTime = DateTime.Now;
            _lastNoDataTime = DateTime.Now;

            HasSpoken = false;

            _controller.Avatar.StopTalking();
        }

        public void SaySomething()
        {
            if (_visibleEntries.Count < 1 || String.IsNullOrEmpty(_visibleEntries[0].Title))
                return;

            var hiddenFor = (DateTime.Now - _lastNoDataTime).TotalMilliseconds;
            if (hiddenFor > 750 || hiddenFor < 200)
                return;

            HasSpoken = true;

            var text = String.Format("Het laatste nieuws van {0}: {1}", _visibleEntries[0].Channel,
                _visibleEntries[0].Title);
            _controller.Avatar.Say(text);

        }

        public void Detach()
        {
            _module = null;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            if (_module == null)
                return;

            if (_module.HasNewData)
            {
                RefreshView();
                _module.HasNewData = false;
            }

            if (_visibleEntries.Count == 0)
            {
                _lastNoDataTime = DateTime.Now;
                RenderLoading(graphics, rectangle);
            }
            else
            {
                if (!HasSpoken)
                    SaySomething();

                RenderNews(graphics, rectangle);
            }
        }

        public void RenderLoading(Graphics graphics, Rectangle rectangle)
        {
            var textRectangle = new Rectangle(200, 55, 1000, 100);
            if ((DateTime.Now - _visibleTime).TotalSeconds > 3)
                graphics.DrawString("Er is tijdelijk geen nieuws beschikbaar.", _titleFont, Brushes.LightGray, textRectangle, _topCenterFormat);
            else
                graphics.DrawString("Laden...", _titleFont, Brushes.White, textRectangle, _topCenterFormat);

        }

        public void RenderNews(Graphics graphics, Rectangle rectangle)
        {
            var yPos = 20;
            var i = 0;
            foreach (var rssEntry in _visibleEntries)
            {
                if (rssEntry.Title == null)
                    continue;

                var titleRect = new Rectangle(200, yPos, 1000, 180);
                var contentRect = new Rectangle(200, yPos + 35, 1000, 94);
                var imgRect = new Rectangle(130, yPos, 50, 50);
                var backgroundRect = new Rectangle(110, yPos - 20, 1220, 200);


                graphics.FillRectangle(_transparentBlack, backgroundRect);

                if (rssEntry.Image != null)
                    DrawImage(graphics, rssEntry.Image, imgRect);

                graphics.DrawString(rssEntry.Title, _titleFont, Brushes.White, titleRect, _topLeftFormat);

                if (rssEntry.Content != null)
                    graphics.DrawString(rssEntry.Content, _baseFont, Brushes.GhostWhite, contentRect, _topLeftFormat);

                if (!String.IsNullOrEmpty(rssEntry.Channel))
                    graphics.DrawString(rssEntry.Channel, _baseFont, Brushes.LightGray, titleRect, _bottomLeftFormat);

                var time = String.Format("{0}, {1}", BackgroundUtils.GetDate(rssEntry.Timestamp),
                    BackgroundUtils.GetTime(rssEntry.Timestamp));
                ;
                graphics.DrawString(time, _dateFont, Brushes.LightGray, titleRect, _bottomRightFormat);

                yPos += 220;
                i++;
            }

        }
        /// <summary>
        /// Resizes an image so it fits in the available space box nicely
        /// Only suitable for square rectangles
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="image"></param>
        /// <param name="availableRectangle"></param>
        private void DrawImage(Graphics graphics, Image image, Rectangle availableRectangle)
        {
            var Size = availableRectangle.Size;
            var Pos = availableRectangle.Location;
            if (!Size.Height.Equals(Size.Width))
                return;

            Size drawSize;
            if (image.Height > image.Width)
                drawSize = new Size((int)Math.Round(1d * image.Width / image.Height * availableRectangle.Width), image.Height);
            else
                drawSize = new Size(image.Width, (int)Math.Round(1d * image.Height / image.Width * availableRectangle.Height));

            var drawRect = new Rectangle(Pos, drawSize);

            graphics.DrawImage(image, drawRect);
        }

        public void RefreshView()
        {
            _visibleEntries.Clear();

            var entryFeedCount = new Dictionary<String, Int32>();

            if (_module == null || _module.entries == null)
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