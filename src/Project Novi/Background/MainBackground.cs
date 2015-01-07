using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Background
{
    public class MainBackground : IBackgroundView
    {
        public void Render(Graphics graphics, Rectangle rectangle, IController controller)
        {
            BackgroundUtils.DrawBackground(graphics);
            BackgroundUtils.DrawClock(graphics);
            BackgroundUtils.DrawWidgets(graphics, controller.ModuleManager.BackgroundWidgets);
        }

        public Rectangle GetModuleRectangle(Rectangle fullRectangle)
        {
            return fullRectangle;
        }


        public void Attach() { }

        public void Detach() { }
    }
}
