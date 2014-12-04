using System;
using System.Collections.Generic;
using System.Drawing;

namespace Project_Novi.Modules.Backgrounds
{
    class MainBackground : IBackgroundView
    {

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            BackgroundUtils.drawBackground(graphics);
            BackgroundUtils.drawClock(graphics);
        }
    }
}
