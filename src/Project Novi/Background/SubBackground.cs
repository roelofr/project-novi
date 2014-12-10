using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Background
{
    public class SubBackground : IBackgroundView
    {
        public void Render(Graphics graphics, Rectangle rectangle)
        {
            BackgroundUtils.DrawBackground(graphics);
            BackgroundUtils.DrawClock(graphics);
        }

        public Rectangle GetModuleRectangle(Rectangle fullRectangle)
        {
            // TODO: Return the actual rectangle to be used by the module.
            return fullRectangle;
        }
    }
}
