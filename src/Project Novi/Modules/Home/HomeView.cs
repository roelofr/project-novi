using System;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Background;
using Project_Novi.Render;
using Project_Novi.Text;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private HomeModule _module;
        private IController _controller;

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
        }

        public void Detach()
        {
            _module = null;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            var rectText = new Rectangle(1, 295, 1920, 90);
            const int fontSize = 45;

            var strFont = TextUtils.GetFont(fontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

            graphics.DrawString(Avatar.Saying, strFont, Brushes.White, rectText, stringFormat);

            var rectAvatar = new Rectangle(rectText.X, 489, 1920, 1080 - 489);
            _controller.Avatar.Render(graphics, rectAvatar);
        }
    }
}
