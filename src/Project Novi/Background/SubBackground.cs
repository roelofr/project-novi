using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Text;

namespace Project_Novi.Background
{
    public class SubBackground : IBackgroundView
    {
        private readonly IController _controller;

        private const int ModuleOffsetX = 500;
        private const int ModuleOffsetY = 200;
        private const int FontSize = 30;
        private const int BackButtonSize = 150;

        private Rectangle _avatar;
        private readonly Rectangle _backButton = new Rectangle(20, 1080 - BackButtonSize, BackButtonSize, BackButtonSize);

        private Rectangle _textRect = new Rectangle(0, 100, ModuleOffsetX, 500);
        private readonly StringFormat _stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        private readonly Font _strFont = TextUtils.GetFont(FontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, FontSize, FontStyle.Regular);

        private bool _keepTextInView = false;

        public SubBackground(IController controller)
        {
            _controller = controller;
        }

        public SubBackground(IController controller, bool keepTextInView)
        {
            _controller = controller;
            _keepTextInView = keepTextInView;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            BackgroundUtils.DrawBackground(graphics);
            BackgroundUtils.DrawClock(graphics);

            _avatar = new Rectangle(rectangle.X + _backButton.Width,
                rectangle.Y + 600,
                ModuleOffsetX - _backButton.Width,
                rectangle.Height - 600);

            _controller.Avatar.Render(graphics, _avatar);
            graphics.DrawImage(Properties.Resources.home_button, _backButton);
            if (_controller.Avatar.Talking || _keepTextInView)
            {
                graphics.DrawString(_controller.Avatar.Saying, _strFont, Brushes.White, _textRect, _stringFormat);
            }
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
            point.X += ModuleOffsetX;
            point.Y += ModuleOffsetY;

            if (_backButton.Contains(point))
            {
                _controller.SelectModule(_controller.ModuleManager.GetModule("Home"));
            }
            else if (_avatar.Contains(point))
            {
                if (_controller.Avatar.Talking)
                {
                    _controller.Avatar.Talking = false;
                }
                else
                {
                    _controller.Avatar.Pinch();
                    _controller.Avatar.Say(TextManager.GetText("Poke"));
                }
            }
        }

        public void Detach() { }
    }
}
