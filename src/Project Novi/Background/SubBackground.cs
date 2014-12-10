using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Background
{
    public class SubBackground : IBackgroundView
    {
        private readonly IController _controller;

        private const int ModuleOffsetX = 500;
        private const int ModuleOffsetY = 200;

        private readonly Rectangle _backButton = new Rectangle(0, 980, 100, 100);

        public SubBackground(IController controller)
        {
            _controller = controller;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            BackgroundUtils.DrawBackground(graphics);
            BackgroundUtils.DrawClock(graphics);
            _controller.Avatar.Render(graphics, new Rectangle(rectangle.X + _backButton.Width, rectangle.Y, ModuleOffsetX - _backButton.Width, rectangle.Height));
            graphics.DrawImage(Properties.Resources.home_button, _backButton);
        }

        public Rectangle GetModuleRectangle(Rectangle fullRectangle)
        {
            var moduleRectangle = fullRectangle;
            moduleRectangle.Width -= ModuleOffsetX;
            moduleRectangle.Height -= ModuleOffsetY;
            moduleRectangle.Offset(ModuleOffsetX, ModuleOffsetY);

            return moduleRectangle;
        }


        public void Attach()
        {
            _controller.Touch += ControllerOnTouch;
        }

        private void ControllerOnTouch(Point point)
        {
            if (_backButton.Contains(point))
            {
                _controller.SelectModule(_controller.ModuleManager.GetModule("Home"));
            }
        }

        public void Detach() { }
    }
}
