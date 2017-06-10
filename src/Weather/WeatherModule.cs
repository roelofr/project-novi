using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ForecastIO;
using Project_Novi.Api;
using Weather.Properties;

namespace Weather
{
    public class WeatherModule : IModule
    {
        private IController _controller;
        internal ForecastIOResponse WeatherResponse;

        private const string CredentialFile = "forecastApi.xml";
        private string ApiKey = null;

        public string Name
        {
            get { return "Weather"; }
        }

        public Bitmap Icon
        {
            get
            {
                if (WeatherResponse != null)
                    return GetWeatherImage(WeatherResponse.currently.icon, true);
                return GetWeatherImage("sunny", true);
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

            // Get config from XML
            try {
                ApiKey = GetApiKeyFromXml();
            } catch {
                ApiKey = null;
            }

            // Update data
            Update();
        }

        private string GetApiKeyFromXml()
        {
            if (!File.Exists(CredentialFile)) return null;

            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(CredentialFile);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception! {0}", e.Message);
                return null;
            }

            var body = xmlDocument.DocumentElement;
            if (body == null) return null;

            var keyNode = body.SelectSingleNode("/forecast/key");

            if (keyNode == null) return null;

            return keyNode.InnerText;
        }

        private void Update()
        {
            if (ApiKey == null) return;

            try
            {
                var request = new ForecastIORequest(ApiKey, 52.5f, 6.079f, Unit.si);
                WeatherResponse = request.Get();
            }
            catch { }
        }

        internal static Bitmap GetWeatherImage(string icon, bool metroIcon)
        {
            if (metroIcon)
            {
                switch (icon)
                {
                    case "clear-day":
                        return Resources.metro_sun_weather;
                    case "clearn-night":
                        return Resources.metro_sun_weather;
                    case "rain":
                        return Resources.metro_raining_weather;
                    case "snow":
                        return Resources.metro_snowing_weather;
                    case "sleet":
                        return Resources.metro_sleet_weather;
                    case "wind":
                        return Resources.metro_cloudy_weather;
                    case "fog":
                        return Resources.metro_cloudy_weather;
                    case "cloudy":
                        return Resources.metro_cloudy_weather;
                    case "partly-cloudy-night":
                        return Resources.metro_mostly_cloudy_weather;
                    default:
                        return Resources.metro_mostly_sunny_weather;
                }
            }

            switch (icon)
            {
                case "clear-day":
                    return Resources.sun_weather;
                case "clearn-night":
                    return Resources.sun_weather;
                case "rain":
                    return Resources.raining_weather;
                case "snow":
                    return Resources.snowing_weather;
                case "sleet":
                    return Resources.sleet_weather;
                case "wind":
                    return Resources.cloudy_weather;
                case "fog":
                    return Resources.cloudy_weather;
                case "cloudy":
                    return Resources.cloudy_weather;
                case "partly-cloudy-night":
                    return Resources.mostly_cloudy_weather;
                default:
                    return Resources.mostly_sunny_weather;
            }
        }

        internal static Bitmap GetWeatherImage(string icon)
        {
            return GetWeatherImage(icon, false);
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
