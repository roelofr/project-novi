﻿using Project_Novi.Modules;
using Project_Novi.Modules.Home;
using Project_Novi.Modules.Map;

namespace Project_Novi
{
    static class ViewFactory
    {
        internal static IView GetView(IModule module, IController controller)
        {
            if (module is HomeModule)
            {
                var v = new HomeView(module as HomeModule, controller);
                return v;
            }
            else if (module is MapModule)
            {
                return new MapView(module as MapModule, controller);
            }
            return null;
        }
    }
}
