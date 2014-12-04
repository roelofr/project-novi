﻿using System.Drawing;

namespace Project_Novi.Modules.Backgrounds
{
    class SubBackground : IBackgroundView
    {
        public void Render(Graphics graphics, Rectangle rectangle)
        {
            BackgroundUtils.DrawBackground(graphics);
            BackgroundUtils.DrawClock(graphics);

        }
    }
}
