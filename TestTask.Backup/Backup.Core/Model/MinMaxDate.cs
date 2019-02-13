using System;

namespace Backup.Core.Model
{
    public class MinMaxDate
    {
        private class ErrorCodes
        {
            public const string INCORRECT_DATE_VALUE_FORMAT = "Incorrect date value {0}";
        }

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public MinMaxDate(DateTime minDate, DateTime maxDate)
        {
            if (minDate > maxDate) throw new ArgumentException(string.Format(ErrorCodes.INCORRECT_DATE_VALUE_FORMAT, minDate));
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public override string ToString()
        {
            return $"{MinDate.Date.ToShortDateString()} - {MaxDate.Date.ToShortDateString()}";
        }
    }
}