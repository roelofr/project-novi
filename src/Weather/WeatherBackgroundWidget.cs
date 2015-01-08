using System;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Text;

namespace Weather
{
    class WeatherBackgroundWidget : IBackgroundWidget
    {
        private WeatherModule _module;

        public string ModuleName
        {
            get { return "Weather"; }
        }

        public void Initialize(IController controller, IModule module)
        {
            _module = (WeatherModule)module;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var str = String.Format("{0}°C", Math.Round(_module.WeatherResponse.currently.temperature));
            var font = TextUtils.GetFont(24);
            var size = graphics.MeasureString(str, font);

            graphics.DrawImage(WeatherModule.GetWeatherImage(_module.WeatherResponse.currently.icon), rectangle.X, (rectangle.Y + 10), rectangle.Height, rectangle.Height);
            graphics.DrawString(str, font, Brushes.White, rectangle.Right - size.Width - 10, (rectangle.Y + 40));
        }
    }
}
