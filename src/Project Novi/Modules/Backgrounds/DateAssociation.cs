using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Modules.Backgrounds
{
    class DateAssociation
    {
        public readonly DateTime date;
        public readonly String description;

        public DateAssociation(DateTime date, String description)
        {
            this.date = date;
            this.description = description;
        }

        public Boolean matches(DateTime check)
        {
            if (check.Month == date.Month && check.Day == date.Day)
                return true;
            return false;
        }
    }
}
