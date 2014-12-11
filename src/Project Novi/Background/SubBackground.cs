using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Render;
using Project_Novi.Text;

namespace Project_Novi.Background
{
    public class SubBackground : IBackgroundView
    {
        private readonly IController _controller;

        private const int ModuleOffsetX = 500;
        private const int ModuleOffsetY = 00;
        private const int FontSize = 35;
        private const int BackButtonSize = 50;

        private readonly Rectangle _backButton = new Rectangle((100/2) - (BackButtonSize/2), 1080 - ((100/2) + (BackButtonSize/2)), BackButtonSize, BackButtonSize);

        private readonly Rectangle _textRect = new Rectangle(0, 100, ModuleOffsetX, 500);
        private readonly StringFormat _stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        private readonly Font _strFont = TextUtils.GetFont(FontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, FontSize, FontStyle.Regular);

        public string AvatarText { get; set; }

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
            graphics.DrawString(Avatar.Saying, _strFont, Brushes.White, _textRect, _stringFormat);
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
