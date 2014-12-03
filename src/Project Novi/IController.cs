using System.Collections.Generic;
using Project_Novi.Modules;
using System.Drawing;

namespace Project_Novi
{
    delegate void TouchHandler(Point point);

    interface IController
    {
        IEnumerator<IModule> GetModules(); 
        void SelectModule(IModule module);
        event TouchHandler Touch;
    }
}
