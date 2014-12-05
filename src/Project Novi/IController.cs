using System.Collections.Generic;
using Project_Novi.Modules;
using System.Drawing;

namespace Project_Novi
{
    delegate void TickHandler();
    delegate void TouchHandler(Point point);
    delegate void DragHandler(Point current, Point origin);

    interface IController
    {
        event TickHandler Tick;
        IEnumerator<IModule> GetModules(); 
        void SelectModule(IModule module);
        event TouchHandler Touch;
        event TouchHandler DragStart;
        event TouchHandler DragEnd;
        event DragHandler Drag;
    }
}
