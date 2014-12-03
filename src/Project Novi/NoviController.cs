using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Project_Novi.Modules;
using Project_Novi.Modules.Home;

namespace Project_Novi
{
    class NoviController : IController
    {
        private readonly Novi _form;
        private IModule _module;

        private Timer _timer;
        public event TickHandler Tick;

        public NoviController(Novi form)
        {
            _form = form;
            _module = new HomeModule();
            _module.Start();
            _form.View = ViewFactory.GetView(_module, this);
            _timer = new Timer { Interval = 10 };
            _timer.Tick += TimerCallback;
            _timer.Start();
        }

        private void TimerCallback(object sender, EventArgs e)
        {
            Tick();
            _form.Invalidate(true);
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
            _form.View = ViewFactory.GetView(_module, this);
        }
    }
}
