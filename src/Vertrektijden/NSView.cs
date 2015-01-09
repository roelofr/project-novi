using System;
using System.Collections.Generic;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Background;

namespace Vertrektijden
{
    internal class NsView : IView
    {
        private const int RowHeight = 40;
        private const int RowMargin = 10;
        private const int TimeWidth = 130;
        private const int DestWidth = 550;
        private const int TrackWidth = 70;
        private const int DelayWidth = 350;
        private IController _controller;
        private NSModule _module;
        private bool _newInformation;

        private readonly StringFormat _alignCenterFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        private readonly StringFormat _alignLeftFormat = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        private readonly StringFormat _alignRightFormat = new StringFormat
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center
        };

        private readonly SolidBrush _blackBrush = new SolidBrush(Color.FromArgb(70, Color.DarkBlue));
        private readonly SolidBrush _lightBlackBrush = new SolidBrush(Color.FromArgb(30, Color.DarkBlue));
        private readonly List<NSReis> _localCacheList = new List<NSReis>();
        private readonly Font _titleFont = new Font("Arial", 40 * 72 / 96, FontStyle.Regular);
        private readonly Font _trainLineFont = new Font("Arial", 20 * 72 / 96, FontStyle.Regular);
        private readonly Font _trainLineLargeFont = new Font("Arial", 20 * 72 / 96, FontStyle.Bold);

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
            if (newsModule == null)
                throw new ArgumentException("A NewsView can only render the interface for a NewsModule");

            _module = newsModule;
            _module.DataUpdated += NewInfoAvailable;

            _module.UpdateData();

            _controller.Avatar.Say("");
        }

        public void Detach()
        {
            _module = null;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            if (_newInformation)
                UpdateInformation();

            var topOffset = (int)(RowHeight * 1.5d);

            var drawLocation = new Rectangle(rectangle.X + 100, rectangle.Y + topOffset, rectangle.Width,
                rectangle.Height - topOffset);

            var contentWidth = TimeWidth + TrackWidth + DestWidth + DelayWidth;

            var rect = new Rectangle(rectangle.X + 100, rectangle.Y, contentWidth, topOffset);
            graphics.DrawString("Actuele vertrektijden voor Zwolle", _titleFont, Brushes.White, rect, _alignCenterFormat);

            if (_localCacheList.Count == 0 || _module.ApiMalConfigured)
                RenderNoData(graphics, drawLocation);
            else
                RenderData(graphics, drawLocation);
        }

        private void NewInfoAvailable(List<NSReis> travelInformation)
        {
            _newInformation = true;
        }

        private void UpdateInformation()
        {
            if (!_newInformation)
                return;

            _newInformation = false;

            if (_module == null)
                return;

            _localCacheList.Clear();

            if (_module.TripList.Count == 0)
                return;

            foreach (var trip in _module.TripList)
            {
                if (trip == null)
                    return;
                _localCacheList.Add(trip);
            }
        }

        public void RenderNoData(Graphics graphics, Rectangle rectangle)
        {
            var font = new Font("Open Sans", 18, FontStyle.Bold);
            var align = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            // Not drawing anything special yet, just a "WIP" message
            String text;
            if (_module.ApiMalConfigured)
                text = "Deze informatie is door een technisch mankement niet beschikbaar.";
            else if (_module.LastDownload.Year > 1970 && (DateTime.Now - _module.LastDownload).TotalSeconds > 10)
                text = "Tijdelijk geen informatie beschikbaar...";
            else
                text = "Informatie wordt opgehaald...";

            var textRect = new Rectangle(rectangle.X, rectangle.Y, TimeWidth + DestWidth + TrackWidth + DelayWidth,
                RowHeight);
            DrawBox(graphics, text, textRect, _trainLineLargeFont, Brushes.White, TextAlign.Center);
        }

        public void RenderData(Graphics graphics, Rectangle rectangle)
        {
            var hasRenderedTrains = false;
            foreach (var trip in _localCacheList)
            {
                Rectangle newLocation;
                if (RenderTrain(graphics, trip, rectangle, out newLocation))
                {
                    hasRenderedTrains = true;
                    rectangle = newLocation;
                    rectangle.Offset(0, RowMargin);
                }

                if (rectangle.Y + RowHeight * 2 > rectangle.Height)
                    break;
            }

            if (hasRenderedTrains)
                return;

            var font = new Font("Open Sans", 18, FontStyle.Bold);
            var align = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            graphics.DrawString("Er is voor vandaag geen reisinformatie meer beschikbaar", font, Brushes.LightGray,
                rectangle, align);
        }

        private bool RenderTrain(Graphics graphics, NSReis reis, Rectangle position, out Rectangle newPosition)
        {
            newPosition = position;
            // Skip if the reis is null, if the destination is unknown or if the train has departed already
            if (reis == null || reis.Destination == null || reis.DepartureTime.CompareTo(DateTime.Now) < 0)
                return false;

            var timeToDeparture = ((reis.DepartureTime - DateTime.Now) + reis.DelayTime).TotalMinutes;
            var aboutToMiss = timeToDeparture < 5;
            var blinkShow = DateTime.Now.Second%10 > 5;


            var timeRect = new Rectangle(position.X, position.Y, TimeWidth, RowHeight);
            var trackRect = new Rectangle(timeRect.X + TimeWidth, position.Y, TrackWidth, RowHeight);
            var destRect = new Rectangle(trackRect.X + TrackWidth, position.Y, DestWidth, RowHeight);
            var delayRect = new Rectangle(destRect.X + DestWidth, position.Y, DelayWidth, RowHeight);
            var infoLine = new Rectangle(destRect.X, position.Y + RowHeight, DestWidth + DelayWidth, RowHeight);
            var trackBrush = reis.DepartureTrack.Modified ? Brushes.Tomato : Brushes.White;

            DrawBox(graphics, BackgroundUtils.GetTime(reis.DepartureTime, false), timeRect, _trainLineLargeFont,
                Brushes.White, TextAlign.Center);
            DrawBox(graphics, reis.DepartureTrack.TrackName, trackRect, _trainLineFont, trackBrush, TextAlign.Center);
            DrawBox(graphics, reis.Destination, destRect, _trainLineLargeFont, Brushes.White, TextAlign.Left);

            if (!string.IsNullOrWhiteSpace(reis.DelayText) && blinkShow)
                DrawBox(graphics, reis.DelayText, delayRect, _trainLineFont, Brushes.Tomato, TextAlign.Left);
            else if (string.IsNullOrWhiteSpace(reis.DelayText) && timeToDeparture < 10)
            {
                var color = timeToDeparture < 6 ? Brushes.Tomato : Brushes.DarkOrange;
                var text = String.Format("Vertrekt over {0} min", Math.Ceiling(timeToDeparture));
                DrawBox(graphics, text, delayRect, _trainLineFont, color, TextAlign.Left);
            } else {
            var text = String.Format("{0} van {1}", reis.TrainType, reis.Provider);
                DrawBox(graphics, text, delayRect, _trainLineFont, Brushes.White, TextAlign.Left);
            }

            newPosition.Offset(0, RowHeight);

            if (aboutToMiss)
                return true;

            if (!string.IsNullOrWhiteSpace(reis.TravelAdvice))
            {
                DrawBox(graphics, reis.TravelAdvice, infoLine, _trainLineFont, Brushes.White, TextAlign.Left, _lightBlackBrush);
                newPosition.Offset(0, RowHeight);
            }
            else if (reis.Comments.Length == 1)
            {
                DrawBox(graphics, reis.Comments[0], infoLine, _trainLineFont, Brushes.White, TextAlign.Left, _lightBlackBrush);
                newPosition.Offset(0, RowHeight);
            }
            else if (reis.Comments.Length > 1)
            {
                var ticker = DateTime.Now.Second % (reis.Comments.Length * 10);
                var text = "";
                for (var i = 0; i < reis.Comments.Length; i++)
                {
                    if (ticker < 10 * i + 10)
                    {
                        text = reis.Comments[i];
                        break;
                    }
                }

                DrawBox(graphics, text, infoLine, _trainLineFont, Brushes.White, TextAlign.Left, _lightBlackBrush);
                newPosition.Offset(0, RowHeight);
            }
            else if (!string.IsNullOrWhiteSpace(reis.TripText))
            {
                DrawBox(graphics, "Reist via " + reis.TripText, infoLine, _trainLineFont, Brushes.White,
                    TextAlign.Left, _lightBlackBrush);
                newPosition.Offset(0, RowHeight);
            }

            return true;
        }

        private void DrawBox(Graphics graphics, string display, Rectangle drawRectangle, Font font, Brush textBrush,
            TextAlign align = TextAlign.Left, SolidBrush backgroundBrush = null)
        {
            if (display == null)
                return;

            backgroundBrush = backgroundBrush ?? _blackBrush;
            var drawRect = new Rectangle(
                drawRectangle.X + 1, drawRectangle.Y + 1,
                drawRectangle.Width - 2, drawRectangle.Height - 2
                );
            graphics.FillRectangle(backgroundBrush, drawRect);


            if (string.IsNullOrWhiteSpace(display))
                return;

            // Add padding
            var textRect = new Rectangle(drawRectangle.X + 10, drawRectangle.Y, drawRectangle.Width - 20,
                drawRectangle.Height);

            StringFormat textAlign;
            if (align == TextAlign.Left)
                textAlign = _alignLeftFormat;
            else if (align == TextAlign.Right)
                textAlign = _alignRightFormat;
            else
                textAlign = _alignCenterFormat;

            graphics.DrawString(display, font, textBrush, textRect, textAlign);
        }

        private enum TextAlign
        {
            Left,
            Center,
            Right
        };
    }
}