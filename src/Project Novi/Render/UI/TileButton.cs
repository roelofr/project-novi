using System;
using System.Drawing;
using Project_Novi.Api;

namespace Project_Novi.Render.UI
{
    class TileButton : Button
    {
        public Boolean IsReleased = true;
        public Bitmap Icon { get; set; }

        public TileButton(IController ctrl, String text, Bitmap icon) : base(ctrl, text) {
            Icon = icon;

            this.Size = new Size(200, 80);
        }

        public new void Render(Graphics graphics)
        {
            Rectangle drawRect = new Rectangle(this.Location, this.Size);

            if (!IsReleased)
            {
                DrawBackground(graphics, drawRect);
                graphics.DrawImage(Properties.Resources.pakket, drawRect);
                return;
            }

            int buttonSize = 64;
            Rectangle iconLocation = new Rectangle(drawRect.X + (int)Math.Floor(drawRect.Width - buttonSize * 1.2), drawRect.Y + (drawRect.Height - buttonSize) / 2, buttonSize, buttonSize);
            Rectangle textLocation = new Rectangle(drawRect.X, drawRect.Y, drawRect.Width - iconLocation.Width, drawRect.Height);

            DrawBackground(graphics, drawRect);

            graphics.DrawImage(Icon, iconLocation);

            DrawText(graphics, Text, textLocation, 15);

        }
    }
}
