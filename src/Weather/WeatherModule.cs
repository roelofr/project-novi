using System.Drawing;
using Project_Novi.Api;
using ForecastIO;

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
            get { return null; }
        }

        public string DisplayName
        {
            get { return "Weer"; }
        }

        public void Initialize(IController controller)
        {
            _controller = controller;
        }

        public void Start()
        {
            var request = new ForecastIORequest("***REMOVED***", 52.5f, 6.079f, Unit.si);
            WeatherResponse = request.Get();
        }

        public void Stop()
        {
            
        }
    }
}
