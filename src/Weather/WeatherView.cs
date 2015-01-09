using System;
using Project_Novi.Api;
using Project_Novi.Background;
using System.Drawing;
using ForecastIO;
using Project_Novi.Text;

namespace Weather
{
    class WeatherView : IView
    {
        private IController _controller;
        private WeatherModule _module;

        private static readonly string[] _directions =
        {
            "N", "NO", "O", "ZO", "Z", "ZW", "W", "NW"
        };

        public Type ModuleType
        {
            get { return typeof(WeatherModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            _controller = controller;
            BackgroundView = new SubBackground(controller);
        }

        public void Attach(IModule module)
        {
            var weatherModule = module as WeatherModule;
            if (weatherModule == null)
                throw new ArgumentException("A WeatherView can only render the interface for a WeatherModule");

            _module = weatherModule;
            try
            {
                _controller.Avatar.Say(String.Format("Het weer vandaag: {0}", _module.WeatherResponse.currently.summary));
            }
            catch { }
        }

        public void Detach()
        {
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static void RenderDay(Graphics graphics, DailyForecast day, Rectangle rectangle)
        {
            var font = TextUtils.GetFont(28);
            var image = WeatherModule.GetWeatherImage(day.icon);
            var width = (int)(rectangle.Width * 0.7);
            var height = (int)(image.Height * ((float)width / image.Width));

            var culture = new System.Globalization.CultureInfo("nl-NL");
            var dayName = culture.DateTimeFormat.GetDayName(UnixTimeStampToDateTime(day.time).DayOfWeek);

            var str = String.Format("{0}\n{1}°C tot {2}°C\n\n\nwindsnelheid: {3} km/u {4}", dayName,
                Math.Round(day.temperatureMin),
                Math.Round(day.temperatureMax),
                Math.Round(day.windSpeed * 3.6),
                _directions[(int)Math.Round(day.windBearing / 360 * 8) % 8]);

            graphics.DrawImage(image, rectangle.X, rectangle.Y, width, height);
            graphics.DrawString(
                str,
                font, Brushes.White,
                new Rectangle(rectangle.X, rectangle.Y + height, width + 2, rectangle.Height - height));
        }

        private static void RenderDay(Graphics graphics, Currently today, Rectangle rectangle)
        {
            var font = TextUtils.GetFont(28);
            var image = WeatherModule.GetWeatherImage(today.icon);
            var width = (int)(rectangle.Width * 0.7);
            var height = (int)(image.Height * ((float)width / image.Width));

            var str = String.Format("{0}\n{1}°C\nvoelt als {2}°C\n\nwindsnelheid:\n{3} km/u {4}", "vandaag",
                Math.Round(today.temperature),
                Math.Round(today.apparentTemperature),
                Math.Round(today.windSpeed * 3.6),
                _directions[(int)Math.Round(today.windBearing / 360 * 8) % 8]);

            graphics.DrawImage(image, rectangle.X, rectangle.Y, width, height);
            graphics.DrawString(
                str,
                font, Brushes.White,
                new Rectangle(rectangle.X, rectangle.Y + height, rectangle.Width, rectangle.Height - height));
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            try
            {
                var dayWidth = rectangle.Width/4;
                int x = 0;

                RenderDay(graphics, _module.WeatherResponse.currently,
                    new Rectangle(x, rectangle.Y, dayWidth, rectangle.Height));
                x += dayWidth;

                foreach (var day in _module.WeatherResponse.daily.data.GetRange(1, 3))
                {
                    RenderDay(graphics, day, new Rectangle(x, rectangle.Y, dayWidth, rectangle.Height));
                    x += dayWidth;
                    if (x >= rectangle.Right) break;
                }

                var font = TextUtils.GetFont(14);
                var str = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
                graphics.DrawString(String.Format("Powered by Forecast (forecast.io)"), font, Brushes.White, rectangle, str);
            }
            catch
            {
                var font = TextUtils.GetFont(28);
                var str = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
                graphics.DrawString(String.Format("Er is geen weeroverzicht beschikbaar op dit moment, excuses voor het ongemak."), font, Brushes.White, rectangle, str );
            }
        }
    }
}
