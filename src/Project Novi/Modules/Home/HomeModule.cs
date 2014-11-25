namespace Project_Novi.Modules.Home
{
    class HomeModule : IModule
    {
        public string Name
        {
            get { return "Home"; }
        }

        public string WelcomeText
        {
            get { return "Hallo, kan ik u helpen?"; }
        }

        public void Start() { }

        public void Stop() { }
    }
}
