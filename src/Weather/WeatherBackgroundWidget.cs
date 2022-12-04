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
            try
            {
                if ( _module.WeatherResponse == null)
                {
                    return;
                }

                var str = String.Format("{0}°C", Math.Round(_module.WeatherResponse.currently.temperature));
                var font = TextUtils.GetFont(24);

                var imageSize = new Size(64, 64);
                var imagePosition = new Rectangle(rectangle.X,
                    (rectangle.Y + 10) + (rectangle.Height - 10 - imageSize.Height)/2, imageSize.Height,
                    imageSize.Height);
                var textRectangle = new Rectangle(imagePosition.X + imagePosition.Width + 10, rectangle.Y,
                    rectangle.Width - imagePosition.Width - 10, rectangle.Height);
                var textFormat = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };

                graphics.DrawImage(WeatherModule.GetWeatherImage(_module.WeatherResponse.currently.icon, false), imagePosition);
                graphics.DrawString(str, font, Brushes.White, textRectangle, textFormat);
            }
            catch { }
        }
    }
}
