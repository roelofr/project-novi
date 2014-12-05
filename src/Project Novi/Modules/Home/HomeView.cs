using System.Drawing;
using Project_Novi.Render;
using Project_Novi.Render.UI;
using System.Windows.Forms;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private readonly HomeModule _module;
        private readonly IController _controller;
        public Avatar Avatar;

        Project_Novi.Render.UI.Button btn;
      
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

            btn = new Project_Novi.Render.UI.Button(controller);
            btn.Location = new Point(1920 / 2, 1080 - 300);
            btn.Click += btn_Click;
        }

        void btn_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Got some bacon!");
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

            btn.Render(graphics);
        }
    }
}
