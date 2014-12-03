using Project_Novi.Modules.Bg_generic;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Project_Novi.Modules.Backgrounds
{
    class MainBackground : IBackgroundView
    {
        private static readonly List<DateAssociation> dates = new List<DateAssociation>();

        private static void fillWithDates() {
            if (dates.Count > 0)
                return;

            insertDateAssoc(25, 12, "1e kerstdag");
        }
        private static Boolean insertDateAssoc(int Day, int Month, String description)
        {
            try
            {
                String timeString = String.Format("%d-%d-%dT00:00:00+01:00", Day, Month, DateTime.Now.Year);
                DateAssociation da = new DateAssociation(DateTime.Parse(timeString), description);
                dates.Add(da);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public MainBackground()
        {
            fillWithDates();
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.DrawImage(Properties.Resources.ontwerpWithBorder, 0, 0, 1920, 1080);
        }
    }
}
