using System;
using System.Collections.Generic;
using Project_Novi.Modules;
using Project_Novi.Modules.Home;

namespace Project_Novi
{
    class NoviController : IController
    {
        private readonly Novi _form;
        private IModule _module;

        public NoviController(Novi form)
        {
            _form = form;
            _module = new HomeModule();
            _form.View = ViewFactory.GetView(_module);
        }

        public IEnumerator<IModule> GetModules()
        {
            throw new NotImplementedException();
        }

        public void SelectModule(IModule module)
        {
            _module.Stop();
            _module = module;
            _module.Start();
            _form.View = ViewFactory.GetView(_module);
        }
    }
}
