using System.Drawing;
using Project_Novi.Render;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private readonly HomeModule _module;
        private readonly IController _controller;
        public Avatar Avatar;
        private FontFamily font = null;
      
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
            StringFormat stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            Rectangle rectText = new Rectangle(1, 295, 1920, 90);
            int fontSize = 45;

            Font strFont = TextUtils.getFont(fontSize);
            if (strFont == null)
                strFont = new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

                graphics.DrawString(_module.AvatarText, strFont, Brushes.White, rectText, stringFormat);

            Rectangle rectAvatar = new Rectangle(rectText.X, 489, 1920, 1080 - 489);
            Avatar.Render(graphics, rectAvatar);
        }
    }
}
