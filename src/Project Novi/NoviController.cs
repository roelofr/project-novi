using System;
using System.Windows.Forms;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Render;

namespace Project_Novi
{
    class NoviController : IController
    {
        private readonly Novi _form;
        private IModule _module;

        public ModuleManager ModuleManager { get; private set; }

        private Timer _timer;
        public Avatar Avatar { get; private set; }
        public event TickHandler Tick;
        public event TouchHandler Touch;

        public NoviController()
        {
            _form = new Novi(this);
            Avatar = new Avatar(this);
            ModuleManager = new ModuleManager(this);
        }

        public Form Start()
        {
            SelectModule(ModuleManager.GetModule("Home"));

            _timer = new Timer { Interval = 10 };
            _timer.Tick += TimerCallback;
            _timer.Start();

            return _form;
        }

        private void TimerCallback(object sender, EventArgs e)
        {
            if (Tick != null) Tick();
            _form.Invalidate(true);
        }

        public void SelectModule(IModule module)
        {
            if (_module != null)
            {
                _module.Stop();
                Tick = null;
                Touch = null;
                _form.View.Detach();
            }

            _module = module;
            _module.Start();
            Avatar.Attach();

            var view = ModuleManager.GetView(_module);
            view.Attach(_module);
            _form.BackgroundView = view.BackgroundView;
            _form.View = view;
        }


        public void HandleTouch(Point point)
        {
            if (Touch != null)
            {
                Touch(point);
            }
        }
    }
}
