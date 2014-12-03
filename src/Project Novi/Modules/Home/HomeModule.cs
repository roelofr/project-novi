namespace Project_Novi.Modules.Home
{
    class HomeModule : IModule
    {
        public string AvatarText = "";
        public string Name
        {
            get { return "Home"; }
        }

        public string WelcomeText
        {
            get { return "Hallo, kan ik u helpen?"; }
        }

        public void Start()
        {
            //Available categories: Welkom, Poke, Idle, Kaart, RouteVragen, RouteBerekenen en BerekendeRoute
            //Let op: Exact overnemen!!!
            AvatarText = Text.TextManager.GetText("Welkom");
            TTS.TTS.TextToSpeech(AvatarText);
        }

        public void Stop() { }
    }
}
