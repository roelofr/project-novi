using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Render.UI
{
    class TileButton : Button
    {
        public Bitmap Icon { get; set; }

        public TileButton(IController ctrl, String text, Bitmap icon) : base(ctrl, text) {
            Icon = icon;

            this.Size = new Size(200, 80);
        }

        public override void Render(Graphics graphics)
        {
            Rectangle drawRect = new Rectangle(this.Location, this.Size);
            Rectangle iconLocation = new Rectangle((int)Math.Floor(drawRect.Width - 128d * .75d), (drawRect.Height - 128) / 2, 128, 128);
            Rectangle textLocation = new Rectangle(drawRect.X, drawRect.Y, drawRect.Width - iconLocation.Width, drawRect.Height);

            DrawBackground(graphics, drawRect);

            graphics.DrawImage(Icon, iconLocation);

            DrawText(graphics, Text, textLocation, 15);

            base.Render(graphics);
        }
    }
}
