using System;
using System.Windows.Forms;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Render;
using System.Collections.Generic;

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

        public event TouchHandler DragStart;
        public event TouchHandler DragEnd;
        public event DragHandler Drag;

        /// <summary>
        /// Set to true when the user touches down on the screen or when the mouse goes down
        /// </summary>
        private bool isMouseDown = false;
        /// <summary>
        /// Contains the location where the drag event started
        /// </summary>
        private Point dragOrigin;
        /// <summary>
        /// Set to true if the user is dragging, causes a DragEnd event to fire on the _controller
        /// </summary>
        private bool isDragging = false;

        /// <summary>
        /// The minimum distance to move before starting a drag
        /// </summary>
        private readonly Point DragMargin = new Point(4, 4);

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
            GoIdle();
        }

        public void SelectModule(IModule module)
        {
            if (_module != null)
            {
                _module.Stop();
                Tick = null;
                Touch = null;
                _form.View.Detach();
                _form.BackgroundView.Detach();
            }

            _module = module;
            _module.Start();
            Avatar.Attach();

            var view = ModuleManager.GetView(_module);
            view.Attach(_module);
            _form.BackgroundView = view.BackgroundView;
            _form.BackgroundView.Attach();
            _form.View = view;
        }

        public void HandleTouch(Point point)
        {
            if (Touch != null)
            {
                Touch(point);
            }
        }

        public void GoIdle()
        {
            if (IdleManager.CheckIdle(_module))
            {
                List<String> moduleNames = ModuleManager.GetModuleNameList();
                List<IModule> modules = new List<IModule>();
                List<IModule> rotatableModules = new List<IModule>();
                foreach (var name in moduleNames)
                {
                    modules.Add(ModuleManager.GetModule(name));
                }
                foreach (var module in modules)
                {
                    if (module.Rotatable && module != _module)
                    {
                        rotatableModules.Add(module);
                    }
                }
                Random rand = new Random();
                var randomModule = rand.Next(0, rotatableModules.Count);
                SelectModule(ModuleManager.GetModule(rotatableModules[randomModule].Name));
            }
        }

        public void HandleTouchStart(Point point)
        {
            isMouseDown = true;
            dragOrigin = point;
        }

        public void HandleTouchMove(Point point)
        {
            if (!isMouseDown)
                return;

            if (!isDragging)
            {
                if (Math.Abs(point.X - dragOrigin.X) > DragMargin.X)
                    isDragging = true;
                else if (Math.Abs(point.Y - dragOrigin.Y) > DragMargin.Y)
                    isDragging = true;
                else
                    return;

                if (DragStart != null)
                    DragStart(dragOrigin);
            }

            if (Drag != null)
                Drag(point, dragOrigin);
        }

        public void HandleTouchEnd(Point point)
        {
            if (isDragging && DragEnd != null)
                DragEnd(point);
                
            isMouseDown = false;
            isDragging = false;
        }
    }
}
