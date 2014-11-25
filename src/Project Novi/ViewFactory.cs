using Project_Novi.Modules;
using Project_Novi.Modules.Home;

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
            return null;
        }
    }
}
