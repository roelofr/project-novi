using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ForecastIO;
using Project_Novi.Api;
using Weather.Properties;

namespace Weather
{
    public class WeatherModule : IModule
    {
        private IController _controller;
        internal ForecastIOResponse WeatherResponse;

        public string Name
        {
            get { return "Weather"; }
        }

        public Bitmap Icon
        {
            get
            {
                if (WeatherResponse != null)
                    return GetWeatherImage(WeatherResponse.currently.icon);
                return null;
            }
        }

        public string DisplayName
        {
            get { return "Weer"; }
        }

        public bool Rotatable
        {
            get { return true; }
        }

        public void Initialize(IController controller)
        {
            _controller = controller;
            _controller.BackgroundUpdate += Update;
            Update();
        }

        private void Update()
        {
            var request = new ForecastIORequest("***REMOVED***", 52.5f, 6.079f, Unit.si);
            WeatherResponse = request.Get();
        }

        internal static Bitmap GetWeatherImage(string icon)
        {
            if (icon == "clear-day") return Resources.sun_weather;
            if (icon == "clearn-night") return Resources.sun_weather;
            if (icon == "rain") return Resources.raining_weather;
            if (icon == "snow") return Resources.snowing_weather;
            if (icon == "sleet") return Resources.sleet_weather;
            if (icon == "wind") return Resources.cloudy_weather;
            if (icon == "fog") return Resources.cloudy_weather;
            if (icon == "cloudy") return Resources.cloudy_weather;
            if (icon == "partly-cloudy-day") return Resources.mostly_sunny_weather;
            if (icon == "partly-cloudy-night") return Resources.mostly_cloudy_weather;

            return Resources.mostly_sunny_weather;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
