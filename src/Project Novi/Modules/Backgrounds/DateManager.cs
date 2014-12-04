using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Modules.Backgrounds
{
    class DateManager
    {
        private static readonly List<DateAssociation> dates = new List<DateAssociation>();

        private static readonly String[] months = new String[]{
            "januari", "februari", "maart",
            "april", "mei", "juni",
            "juli", "augustus", "september",
            "oktober", "november", "december"
        };
        private static readonly String[] daysOfWeeks = new String[]{
            "zondag", "maandag", "dinsdag","woensdag","donderdag","vrijdag","zaterdag"
        };

        private static DateTime currentDate = DateTime.MinValue;
        private static DateAssociation currentAssociation;

        private static void fillWithDates()
        {
            if (dates.Count > 0)
                return;

            insertDateAssoc(4, 12, "Sinterklaasavond");
            insertDateAssoc(5, 12, "Sinterklaasavond");
            insertDateAssoc(25, 12, "1e kerstdag");
            insertDateAssoc(26, 12, "2e kerstdag");
            insertDateAssoc(1, 1, "nieuwjaarsdag");
            insertDateAssoc(31, 12, "oudejaarsdag");
            insertDateAssoc(27, 4, "Koningsdag");
            insertDateAssoc(1, 5, "Dag van de Arbeid");
            insertDateAssoc(4, 5, "Dodenherdenking");
            insertDateAssoc(5, 5, "Bevrijdingsdag");
        }
        private static String zeroFill(int value)
        {
            if (value < 10)
                return String.Format("0{0}", value);
            return value.ToString();
        }
        private static Boolean insertDateAssoc(int Day, int Month, String description)
        {
            try
            {
                String timeString = String.Format("{2}-{1}-{0} 00:00:00Z", zeroFill(Day), zeroFill(Month), DateTime.Now.Year);
                Console.WriteLine("Parsing {0} for {1}", timeString, description);

                DateTime output = DateTime.ParseExact(timeString, "u", System.Globalization.CultureInfo.InvariantCulture);
                DateAssociation da = new DateAssociation(output, description);

                dates.Add(da);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateAssociation getDateAssociation()
        {
            DateTime now = DateTime.Now;
            if (currentDate == now.Date)
                return currentAssociation;

            fillWithDates();

            foreach (DateAssociation da in dates)
            {
                Console.WriteLine("Checking {0} if it matches {1}", da.date.Date, DateTime.Now.Date);
                if (da.matches(DateTime.Now))
                {
                    Console.WriteLine("{0} ({1} matches!", da.date.Date, da.description);
                    currentAssociation = da;
                    break;
                }
            }

            currentDate = DateTime.Now.Date;
            return currentAssociation;
        }

        public static String getDateAssociationText()
        {
            var curAssoc = getDateAssociation();
            if (curAssoc == null)
                return "";
            return curAssoc.description;
        }

        public static String getDate()
        {
            DateTime now = DateTime.Now;
            return String.Format("{0} {1} {2} {3}", daysOfWeeks[(int)now.DayOfWeek], now.Day, months[now.Month - 1], now.Year);
        }

        public static String getTime()
        {
            DateTime now = DateTime.Now;
            return String.Format("{0}:{1}:{2}", now.Hour, now.Minute, now.Second);

        }

        public DateManager()
        {
            // Doesn't do anything
        }
    }
}
