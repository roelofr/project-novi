using System.Drawing;
using Project_Novi.Render;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private readonly HomeModule _module;
        private readonly IController _controller;
        public Avatar Avatar;
      
        public IModule Module
        {
            get { return _module; }
        }

        public HomeView(HomeModule module, IController controller)
        {
            _module = module;
            _controller = controller;
            Avatar = new Avatar(_controller);
            Avatar.Say(_module.AvatarText);
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            var rectText = new Rectangle(1, 295, 1920, 90);
            const int fontSize = 45;

            var strFont = TextUtils.GetFont(fontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

            graphics.DrawString(_module.AvatarText, strFont, Brushes.White, rectText, stringFormat);

            var rectAvatar = new Rectangle(rectText.X, 489, 1920, 1080 - 489);
            Avatar.Render(graphics, rectAvatar);
        }
    }
}
