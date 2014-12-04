using System;

namespace Project_Novi.Modules.Backgrounds
{
    class DateAssociation
    {
        public readonly DateTime Date;
        public readonly String Description;

        public DateAssociation(DateTime date, String description)
        {
            Date = date;
            Description = description;
        }

        public Boolean matches(DateTime check)
        {
            return check.Month == Date.Month && check.Day == Date.Day;
        }
    }
}
