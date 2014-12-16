using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Background
{
    public class MainBackground : IBackgroundView
    {
        public void Render(Graphics graphics, Rectangle rectangle)
        {
            BackgroundUtils.DrawBackground(graphics);
            BackgroundUtils.DrawClock(graphics);
        }

        public Rectangle GetModuleRectangle(Rectangle fullRectangle)
        {
            return fullRectangle;
        }


        public void Attach() { }

        public void Detach() { }
    }
}
