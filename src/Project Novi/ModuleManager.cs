using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Project_Novi.Api;
using Project_Novi.Modules.Home;

namespace Project_Novi
{
    public class ModuleManager
    {
        private readonly IController _controller;

        private readonly Dictionary<string, IModule> _modules = new Dictionary<string, IModule>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<Type, IView> _views = new Dictionary<Type, IView>();

        public ModuleManager(IController controller)
        {
            _controller = controller;
            LoadModules();
        }

        public IModule GetModule(string name)
        {
            return _modules[name];
        }

        /// <summary>
        /// Returns a List consisting of the names of all registered modules
        /// </summary>
        /// <returns></returns>
        public List<String> GetModuleNameList()
        {
            var output = new List<String>();
            foreach (var entry in _modules)
            {
                output.Add(entry.Key);
            }
            return output;
        } 

        public IView GetView(IModule module)
        {
            return _views[module.GetType()];
        }

        internal void AddModule(IModule module)
        {
            module.Initialize(_controller);
            if (!_modules.ContainsKey(module.Name))
            {
                _modules.Add(module.Name, module);
            }
        }

        internal void AddView(IView view)
        {
            view.Initialize(_controller);
            if (!_views.ContainsKey(view.ModuleType))
            {
                _views.Add(view.ModuleType, view);
            }
        }

        internal void LoadLocalModules()
        {
            AddModule(new HomeModule());
            AddView(new HomeView());
        }

        internal void LoadModules()
        {
            LoadLocalModules();
            var path = Application.StartupPath + @"\modules\";
            Directory.CreateDirectory(path);
            var pluginFiles = Directory.GetFiles(path, @"*.dll", SearchOption.AllDirectories);

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            foreach (var args in pluginFiles)
            {
                var assembly = Assembly.LoadFile(args);
                try
                {
                    foreach (var t in assembly.GetTypes())
                    {
                        if (t.GetInterface(typeof (IModule).Name) != null)
                        {
                            AddModule((IModule) Activator.CreateInstance(t));
                        }

                        if (t.GetInterface(typeof (IView).Name) != null)
                        {
                            AddView((IView) Activator.CreateInstance(t));
                        }
                    }
                }
                catch { }
            }

        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            char[] splitter = { ',' };
            var name = args.Name.Split(splitter)[0] + ".dll";
            var file =
                Directory.GetFiles(Application.StartupPath, name, SearchOption.AllDirectories)[0];
            return Assembly.LoadFrom(file);
        }
    }
}
