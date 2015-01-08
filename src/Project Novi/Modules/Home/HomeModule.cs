using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Modules.Home
{
    class HomeModule : IModule
    {
        public string AvatarText = "";
        private IController _controller;
        public string Name
        {
            get { return "Home"; }
        }

        public Bitmap Icon
        {
            get { return null; }
        }

        public string DisplayName
        {
            get { return "Home"; }
        }

        public bool Rotatable
        {
            get { return true; }
        }

        public void Initialize(IController controller)
        {
            _controller = controller;
        }

        public void Start()
        {
            //Available categories: Welkom, Poke, Idle, Kaart, RouteVragen
            //Let op: Exact overnemen!!!
            AvatarText = Text.TextManager.GetText("Welkom");
        }

        public void Stop() { }
    }
}
