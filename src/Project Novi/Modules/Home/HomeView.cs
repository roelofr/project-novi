using System;
using System.Drawing;
using System.Xml;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private readonly HomeModule _module;
        public IModule Module
        {
            get { return _module; }
        }

        public HomeView(HomeModule module)
        {
            _module = module;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.Clear(Color.FromArgb(255, 32, 103, 178));

            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            Rectangle rect1 = new Rectangle(1, 1, 1920, 300);
            Font strFont = new Font("Sergoe UI", 50);
            var strTxt = Text.TextManager.GetText("Welkom");

            var stringSize = graphics.MeasureString(strTxt, strFont);
            graphics.DrawString(strTxt, strFont, Brushes.White, rect1, stringFormat);
        }
    }
}
