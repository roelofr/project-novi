using System;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Background;
using Project_Novi.Text;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private HomeModule _module;
        private IController _controller;

        private Rectangle _rectText;
        private Rectangle _rectAvatar;

        public Type ModuleType
        {
            get { return typeof(HomeModule); }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            _controller = controller;
            BackgroundView = new MainBackground();
        }

        public void Attach(IModule module)
        {
            var homeModule = module as HomeModule;
            if (homeModule != null)
            {
                _module = homeModule;
                _controller.Avatar.Say(_module.AvatarText);
            }
            else
                throw new ArgumentException("A MapView can only render the interface for a MapModule");

            _controller.Touch += ControllerOnTouch;
        }

        private void ControllerOnTouch(Point point)
        {
           
            if (_rectAvatar.Contains(point))
            {
                _controller.Avatar.Pinch();

                _controller.Avatar.Say(Text.TextManager.GetText("Poke"));
            }
        }

        public void Detach()
        {
            _module = null;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            _rectText = new Rectangle(1, 295, 1920, 90);
            const int fontSize = 45;

            var strFont = TextUtils.GetFont(fontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

            graphics.DrawString(_module.AvatarText, strFont, Brushes.White, _rectText, stringFormat);

            _rectAvatar = new Rectangle(_rectText.X + ((1920/2) - 250), 489, 500, 1080 - 489);
            _controller.Avatar.Render(graphics, _rectAvatar);
        }
    }
}
