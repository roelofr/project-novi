using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Background
{
    class MainBackground : IBackgroundView
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
    }
}
