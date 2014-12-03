namespace Project_Novi.Modules.Home
{
    class HomeModule : IModule
    {
        private IController _controller;
        public string Name
        {
            get { return "Home"; }
        }

        public string WelcomeText
        {
            get { return "Hallo, kan ik u helpen?"; }
        }

        public HomeModule(IController controller)
        {
            _controller = controller;
            controller.Touch += controller_Touch;
        }

        private void controller_Touch(System.Drawing.Point point)
        {
            _controller.SelectModule(new Map.MapModule(_controller));
        }

        public void Start() { }

        public void Stop() { }
    }
}
