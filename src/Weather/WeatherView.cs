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

        public Type ModuleType
        {
            get { return typeof (WeatherModule); }
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
            var font = TextUtils.GetFont(30);
            var image = WeatherModule.GetWeatherImage(day.icon);
            var width = (int)(rectangle.Width * 0.7);
            var height = (int)(image.Height * ((float)width / image.Width));

            graphics.DrawImage(image, rectangle.X, rectangle.Y, width, height);
            graphics.DrawString(String.Format("{0}\n{1}°C tot {2}°C", UnixTimeStampToDateTime(day.time).DayOfWeek, Math.Round(day.temperatureMin), Math.Round(day.temperatureMax)),
                font, Brushes.White,
                new Rectangle(rectangle.X, rectangle.Y + height, width, rectangle.Height - height));
            
        }

        private static void RenderDay(Graphics graphics, Currently today, Rectangle rectangle)
        {
            var font = TextUtils.GetFont(30);
            var image = WeatherModule.GetWeatherImage(today.icon);
            var width = (int)(rectangle.Width*0.7);
            var height = (int)(image.Height * ((float)width / image.Width));

            graphics.DrawImage(image, rectangle.X, rectangle.Y, width, height);
            graphics.DrawString(String.Format("{0}\n{1}°C\nvoelt als {2}°C", "Vandaag", Math.Round(today.temperature), Math.Round(today.apparentTemperature)),
                font, Brushes.White,
                new Rectangle(rectangle.X, rectangle.Y + height, rectangle.Width, rectangle.Height - height));
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var dayWidth = rectangle.Width/ 4;
            int x = 0;

            RenderDay(graphics, _module.WeatherResponse.currently, new Rectangle(x, rectangle.Y, dayWidth, rectangle.Height));
            x += dayWidth;

            foreach (var day in _module.WeatherResponse.daily.data.GetRange(1, 3))
            {
                RenderDay(graphics, day, new Rectangle(x, rectangle.Y, dayWidth, rectangle.Height));
                x += dayWidth;
                if (x >= rectangle.Right) break;
            }
        }
    }
}
