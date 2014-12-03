using Project_Novi.Modules;
using Project_Novi.Modules.Home;

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
            return null;
        }
    }
}
