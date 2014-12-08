using System.Drawing;
using Project_Novi.Render;

namespace Project_Novi.Api
{
    public delegate void TickHandler();

    public delegate void TouchHandler(Point point);

    public interface IController
    {
        ModuleManager ModuleManager { get; }
        void SelectModule(IModule module);

        Avatar Avatar { get; }

        event TickHandler Tick;
        event TouchHandler Touch;
    }
}
