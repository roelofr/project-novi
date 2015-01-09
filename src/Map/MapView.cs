using System;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Background;
using Project_Novi.Text;

namespace Map
{
    class MapView : IView
    {

        private readonly SolidBrush _markerBrush = new SolidBrush(Color.FromArgb(100, Color.Red));
        private readonly SolidBrush _arrowBrush = new SolidBrush(Color.FromArgb(100, Color.Black));
        private const int MarkerSize = 50;
        private readonly Point[] _activeFloorArrow = new Point[3];

        private MapModule _module;
        private IController _controller;
      
        public Type ModuleType
        {
            get { return typeof(MapModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            BackgroundView = new SubBackground(controller, true);
            _controller = controller;
        }

        public void Attach(IModule module)
        {
            var mapModule = module as MapModule;
            if (mapModule != null)
            {
                _module = mapModule;
                _controller.Touch += ControllerOnTouch;
            }
            else
                throw new ArgumentException("A MapView can only render the interface for a MapModule");


            _controller.Avatar.Say(TextManager.GetText("RouteVragen"));
            
        }

        public void Detach()
        {
            _module = null;
        }

        private void ControllerOnTouch(Point p)
        {
            // Check if a floor button has been pressed
            foreach (var button in _module.FloorButtons)
            {
                if (button.IsClicked(p))
                {
                    _module.HandleTouchButton(MapModule.ButtonType.FloorButton, button);
                }
            }

            // Check if a button in the output field has been pressed
            foreach (var button in _module.FloorSelectOutput.TouchButtons)
            {
                if (button.IsClicked(p))
                {
                    _module.HandleTouchButton(MapModule.ButtonType.OutputButton, button);
                }
            }

            // Check if a button on the numpad has been pressed
            foreach (var button in _module.FloorSelectNumpad.TouchButtons)
            {
                if (button.IsClicked(p))
                {
                    _module.HandleTouchButton(MapModule.ButtonType.NumpadButton, button);
                }
            }

            // Check if backspace has been pressed
            if (_module.Backspace.IsClicked(p))
            {
                _module.HandleTouchButton(MapModule.ButtonType.Backspace, null);
            }

            // Check if the map has been pressed
            if (p.X > _module.XposMap + _module.MarginMap && p.X < _module.XposMap + _module.MarginMap + Properties.Resources.T1x.Width*_module.Scale / 100 && p.Y > _module.YposMap + _module.MarginMap && p.Y < _module.YposMap + _module.MarginMap + Properties.Resources.T1x.Height * _module.Scale / 100)
            {
                _module.HandleTouchMap(p);
            }
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.TranslateTransform(rectangle.X, rectangle.Y);
            _module.FloorSelectNumpad.DrawTouchPad(graphics);
            _module.FloorSelectOutput.DrawNumPadOutput(graphics);
            _module.Backspace.Draw(graphics);
            _module.PointIndicator.Draw(graphics);
            _module.BuildingIndicator.Draw(graphics);

            var bm = _module.GetDisplayMap();
            graphics.FillRectangle(_arrowBrush, _module.XposMap, _module.YposMap, bm.Width + 2 * _module.MarginMap, bm.Height + 2 * _module.MarginMap);
            graphics.DrawImage(bm, _module.XposMap + _module.MarginMap, _module.YposMap + _module.MarginMap, bm.Width * _module.Scale / 100, bm.Height * _module.Scale / 100);

            // Display all floor buttons
            foreach (var button in _module.FloorButtons) {
                if (_module.ActiveFloor.Equals("T" + (_module.FloorNames.Length - 1 - _module.FloorButtons.IndexOf(button))))
                {
                    _activeFloorArrow[0].X = button.Xpos;
                    _activeFloorArrow[0].Y = button.Ypos - 2;
                    _activeFloorArrow[1].X = button.Xpos;
                    _activeFloorArrow[1].Y = button.Ypos + _module.HeightFloorButtons + 1;
                    _activeFloorArrow[2].X = button.Xpos - (5 * _module.MarginFloorButtons);
                    _activeFloorArrow[2].Y = (button.Ypos + button.Ypos + _module.HeightFloorButtons) / 2;
                }                
                button.Draw(graphics);
            }

            // Display arrow for active floor
            if (_module.FloorTimer.IsRunning && _module.FloorTimer.ElapsedMilliseconds / 500 % 2 == 0)
            {
                graphics.FillPolygon(_arrowBrush, _activeFloorArrow);
            }
            
            // Display marker if a room has been selected
            if (_module.FloorSelectOutput.Output.Length == _module.FloorSelectOutput.Digits)
            {
                if (!_module.ActiveTimer.IsRunning)
                {
                    _module.ActiveTimer.Start();
                }
                else if (_module.ActiveTimer.ElapsedMilliseconds / 500 % 2 == 0)
                {
                    graphics.FillEllipse(_markerBrush,
                        _module.ActivePoint.X + _module.MarginMap - (MarkerSize / 2),
                        _module.ActivePoint.Y + _module.MarginMap - (MarkerSize / 2), MarkerSize, MarkerSize);
                    graphics.DrawEllipse(new Pen(_markerBrush.Color, 5), _module.ActivePoint.X + _module.MarginMap - (MarkerSize / 2),
                        _module.ActivePoint.Y + _module.MarginMap - (MarkerSize / 2), MarkerSize, MarkerSize);
                }
            }
            else
            {
                _module.ActiveTimer.Reset();
            }
        }

    }
}
