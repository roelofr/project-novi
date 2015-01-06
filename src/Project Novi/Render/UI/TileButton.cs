using System;
using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Render.UI
{
    class TileButton : Button
    {
        private const int IconSize = 64;

        public Boolean IsReleased = true;
        public Bitmap Icon { get; set; }

        public TileButton(IController ctrl, String text, Bitmap icon)
            : base(ctrl, text)
        {
            Icon = icon;

            this.Size = new Size(200, 80);
        }

        public new void Render(Graphics graphics)
        {
            // If this module is not released (and thusly has no module associated with it), hide it
            if (!IsReleased)
                return;

            var drawRect = new Rectangle(this.Location, this.Size);
            
            //Check if this button is rectangular or square
            var w = (int) drawRect.Width;
            var h = (int) drawRect.Height;

            Rectangle iconLocation;
            Rectangle textLocation;

            if (w > h)
            {
                // Button is rectangular where width > height
                iconLocation = new Rectangle(
                    drawRect.X + (int)Math.Floor(drawRect.Width - IconSize * 1.2),
                    drawRect.Y + (drawRect.Height - IconSize) / 2,
                    IconSize, IconSize
                );

                textLocation = new Rectangle(
                    drawRect.X, drawRect.Y,
                    drawRect.Width - iconLocation.Width, drawRect.Height
                );

            }
            else
            {
                // Button is square or height > width
                // Determine icon position
                var iconTopPos = (int)Math.Max(drawRect.Height / 4 - IconSize / 2, IconSize * .2);
                // Set icon location and text location based on icon location
                iconLocation = new Rectangle(
                    drawRect.X + (int)Math.Floor((drawRect.Width - IconSize) * .5),
                    drawRect.Y + iconTopPos,
                    IconSize, IconSize
                );

                textLocation = new Rectangle(
                    drawRect.X, drawRect.Y + iconTopPos + IconSize,
                    drawRect.Width, drawRect.Height - iconTopPos - IconSize
                );
            }
            DrawBackground(graphics, drawRect);

            graphics.DrawImage(Icon, iconLocation);

            DrawText(graphics, Text, textLocation, 22);

        }
    }
}
