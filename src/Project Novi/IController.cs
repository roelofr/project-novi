using System.Collections.Generic;
using Project_Novi.Modules;

namespace Project_Novi
{
    delegate void TickHandler();
    interface IController
    {
        event TickHandler Tick;
        IEnumerator<IModule> GetModules(); 
        void SelectModule(IModule module);
    }
}
