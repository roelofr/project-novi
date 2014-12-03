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
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            var rect = new Rectangle(1, 1, 1920, 300);
            var strFont = new Font("Sergoe UI", 50);

            graphics.DrawString(_module.AvatarText, strFont, Brushes.White, rect, stringFormat);
            rectangle.Y += 200;
            rectangle.Height -= 200;
            Avatar.Render(graphics, rectangle);
        }
    }
}
