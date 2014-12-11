using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Project_Novi.Modules;
using Project_Novi.Modules.Home;
using System.Drawing;

namespace Project_Novi
{
    class NoviController : IController
    {
        private readonly Novi _form;
        private IModule _module;

        private Timer _timer;
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

        public NoviController(Novi form)
        {
            _form = form;

            _module = new HomeModule(this);
            _module.Start();

            _form.View = ViewFactory.GetView(_module, this);
            _form.BackgroundView = ViewFactory.GetBackgroundView(_form.View);

            _timer = new Timer { Interval = 10 };
            _timer.Tick += TimerCallback;
            _timer.Start();
        }

        private void TimerCallback(object sender, EventArgs e)
        {
            if (Tick != null) Tick();
            _form.Invalidate(true);
        }

        public IEnumerator<IModule> GetModules()
        {
            throw new NotImplementedException();
        }

        public void SelectModule(IModule module)
        {
            _module.Stop();
            Tick = null;
            Touch = null;
            _module = module;
            _module.Start();
            _form.View = ViewFactory.GetView(_module, this);
        }

        public void TouchStart(Point location)
        {

        }


        public void HandleTouch(Point point)
        {
            if (Touch != null)
            {
                Touch(point);
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
