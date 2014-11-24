namespace Project_Novi.Modules
{
    interface IView
    {
        IModule Module { get; }
        void Render();
    }
}
