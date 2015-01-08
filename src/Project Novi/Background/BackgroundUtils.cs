using System;
using System.Collections.Generic;
using System.Drawing;
using Project_Novi.Api;
using System.Globalization;
using Project_Novi.Properties;
using Project_Novi.Text;

namespace Project_Novi.Background
{
    public class BackgroundUtils
    {
        private static readonly List<DateAssociation> Dates = new List<DateAssociation>();

        private static readonly String[] Months =
        {
            "januari", "februari", "maart",
            "april", "mei", "juni",
            "juli", "augustus", "september",
            "oktober", "november", "december"
        };

        private static readonly String[] DaysOfWeeks =
        {
            "zondag", "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag"
        };

        private static DateTime _currentDate = DateTime.MinValue;
        private static DateAssociation _currentAssociation;

        private static void FillWithDates()
        {
            if (Dates.Count > 0)
                return;

            InsertDateAssoc(4, 12, "Sinterklaasavond");
            InsertDateAssoc(5, 12, "Sinterklaasavond");
            InsertDateAssoc(25, 12, "1e kerstdag");
            InsertDateAssoc(26, 12, "2e kerstdag");
            InsertDateAssoc(1, 1, "Nieuwjaarsdag");
            InsertDateAssoc(31, 12, "Oudjaarsdag");
            InsertDateAssoc(27, 4, "Koningsdag");
            InsertDateAssoc(1, 5, "Dag van de Arbeid");
            InsertDateAssoc(4, 5, "Dodenherdenking");
            InsertDateAssoc(5, 5, "Bevrijdingsdag");
        }

        private static String ZeroFill(int value)
        {
            if (value < 10)
                return String.Format("0{0}", value);
            return value.ToString();
        }

        private static bool InsertDateAssoc(int Day, int Month, String description)
        {
            try
            {
                var timeString = String.Format("{2}-{1}-{0} 00:00:00Z", ZeroFill(Day), ZeroFill(Month),
                    DateTime.Now.Year);
                Console.WriteLine("Parsing {0} for {1}", timeString, description);

                var output = DateTime.ParseExact(timeString, "u", CultureInfo.InvariantCulture);
                var da = new DateAssociation(output, description);

                Dates.Add(da);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateAssociation GetDateAssociation()
        {
            var now = DateTime.Now;
            if (_currentDate == now.Date)
                return _currentAssociation;

            FillWithDates();

            foreach (var da in Dates)
            {
                Console.WriteLine("Checking {0} if it matches {1}", da.Date.Date, DateTime.Now.Date);
                if (da.matches(DateTime.Now))
                {
                    Console.WriteLine("{0} ({1} matches!", da.Date.Date, da.Description);
                    _currentAssociation = da;
                    break;
                }
            }

            _currentDate = DateTime.Now.Date;
            return _currentAssociation;
        }

        public static String GetDateAssociationText()
        {
            var curAssoc = GetDateAssociation();
            return curAssoc == null ? "" : curAssoc.Description;
        }

        public static String GetDate()
        {
            return GetDate(DateTime.Now);
        }

        public static String GetDate(DateTime time)
        {
            return String.Format("{0} {1} {2} {3}", DaysOfWeeks[(int) time.DayOfWeek], time.Day, Months[time.Month - 1],
                time.Year);
        }

        public static String GetTime()
        {
            return GetTime(DateTime.Now);
        }

        public static String GetTime(DateTime time)
        {
            return String.Format("{0}:{1}:{2}", time.Hour, ZeroFill(time.Minute), ZeroFill(time.Second));
        }

        /**
         * Draw functions
         */

        public static void DrawBackground(Graphics g)
        {
            g.DrawImage(Resources.ontwerpWithBorder, 0, 0, 1920, 1080);
        }

        public static void DrawClock(Graphics graphics)
        {
            var timeFormat = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center};
            var dateFormat = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center};

            var timeRect = new Rectangle(1700, 40, 300, 40);
            var dateRect = new Rectangle(0, 40, 1700, 40);
            var specialDayRect = new Rectangle(dateRect.X, dateRect.Y + 40, dateRect.Width, dateRect.Height);

            var timeText = GetTime();
            var dateText = GetDate();
            var specialText = GetDateAssociationText();

            Brush specialBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            const int fontSize = 24;

            var strFont = TextUtils.GetFont(fontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

            graphics.DrawString(timeText, strFont, Brushes.White, timeRect, timeFormat);
            graphics.DrawString(dateText, strFont, Brushes.White, dateRect, dateFormat);
            graphics.DrawString(specialText, strFont, specialBrush, specialDayRect, dateFormat);
        }

        public static void DrawWidgets(Graphics graphics, List<IBackgroundWidget> widgets)
        {
            int widgetX = 100;
            foreach (var widget in widgets)
            {
                var rect = new Rectangle(widgetX, 5, 200, 100);
                widget.Render(graphics, rect);
                widgetX += 200;
            }
        }
    }
}
