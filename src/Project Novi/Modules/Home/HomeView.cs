using System.Drawing;

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
            // TODO: Don't simulate a BSOD
            graphics.Clear(Color.FromArgb(255, 32, 103, 178));
            graphics.DrawString(":(", new Font("Segoe UI", 200), Brushes.White, 200, 100);
        }
    }
}
