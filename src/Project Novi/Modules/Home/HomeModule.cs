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

        public void Initialize(IController controller)
        {
            _controller = controller;
        }

        private void controller_Touch(System.Drawing.Point point)
        {
            _controller.SelectModule(_controller.ModuleManager.GetModule("map"));
        }

        public void Start()
        {
            _controller.Touch += controller_Touch;
            //Available categories: Welkom, Poke, Idle, Kaart, RouteVragen, RouteBerekenen en BerekendeRoute
            //Let op: Exact overnemen!!!
            AvatarText = Text.TextManager.GetText("Welkom");
        }

        public void Stop() { }
    }
}
