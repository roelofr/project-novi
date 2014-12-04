using System;
using System.Collections.Generic;
using System.Drawing;

namespace Project_Novi.Modules.Backgrounds
{
    class SubBackground : IBackgroundView
    {

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.DrawImage(Properties.Resources.ontwerpWithBorder, 0, 0, 1920, 1080);

            StringFormat timeFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            StringFormat dateFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

            Rectangle timeRect = new Rectangle(1620, 40, 300, 40);
            Rectangle dateRect = new Rectangle(0, 40, 1620, 40);
            Rectangle specialDayRect = new Rectangle(dateRect.X, dateRect.Y + 40, dateRect.Width, dateRect.Height);

            DateTime now = DateTime.Now;

            String timeText = DateManager.getTime();
            String dateText = DateManager.getDate();
            String specialText = DateManager.getDateAssociationText();

            Brush specialBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            int fontSize = 24;

            Font strFont = TextUtils.getFont(fontSize);
            if (strFont == null)
                strFont = new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

            graphics.DrawString(timeText, strFont, Brushes.White, timeRect, timeFormat);
            graphics.DrawString(dateText, strFont, Brushes.White, dateRect, dateFormat);
            graphics.DrawString(specialText, strFont, specialBrush, specialDayRect, dateFormat);
        }
    }
}
