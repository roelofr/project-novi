namespace Project_Novi.Modules
{
    interface IModule
    {
        string Name { get; }
        string WelcomeText { get; }

        void Start();
        void Stop();
        void GoIdle();
    }
}
