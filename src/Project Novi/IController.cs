using System.Collections.Generic;
using Project_Novi.Modules;

namespace Project_Novi
{
    interface IController
    {
        IEnumerator<IModule> GetModules(); 
        void SelectModule(IModule module);
    }
}
