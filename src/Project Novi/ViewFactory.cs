using Project_Novi.Modules;
using Project_Novi.Modules.Home;
using Project_Novi.Modules.Map;

namespace Project_Novi
{
    static class ViewFactory
    {
        internal static IView GetView(IModule module)
        {
            if (module is HomeModule)
            {
                return new HomeView(module as HomeModule);
            }
            else if (module is MapModule)
            {
                return new MapView(module as MapModule);
            }
            return null;
        }
    }
}
