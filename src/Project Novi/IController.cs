using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Render;

namespace Project_Novi
{
    delegate void TickHandler();
    delegate void TouchHandler(Point point);

    interface IController
    {
        ModuleManager ModuleManager { get; }
        void SelectModule(IModule module);

        Avatar Avatar { get; }

        event TickHandler Tick;
        event TouchHandler Touch;
    }
}
