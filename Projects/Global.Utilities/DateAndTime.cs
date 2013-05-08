using System;
using System.Collections.Generic;
using System.Text;

using Global.Utilities.Enums;

namespace Global.Utilities
{
    public class DateAndTime
    {
        /// <summary>
        /// Returns date2 - date1 depending on specified interval.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2)
        {
            return DateDiff(interval, dt1, dt2, System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
        }

        private static int GetQuarter(int month)
        {
            if (month <= 3)
                return 1;
            if (month <= 6)
                return 2;
            if (month <= 9)
                return 3;
            return 4;
        }

        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2, DayOfWeek eFirstDayOfWeek)
        {
            if (interval == DateInterval.Year)
                return dt2.Year - dt1.Year;

            if (interval == DateInterval.Month)
                return (dt2.Month - dt1.Month) + (12 * (dt2.Year - dt1.Year));

            TimeSpan ts = dt2 - dt1;

            if (interval == DateInterval.Day || interval == DateInterval.DayOfYear)
                return Round(ts.TotalDays);

            if (interval == DateInterval.Hour)
                return Round(ts.TotalHours);

            if (interval == DateInterval.Minute)
                return Round(ts.TotalMinutes);

            if (interval == DateInterval.Second)
                return Round(ts.TotalSeconds);

            if (interval == DateInterval.Weekday)
            {
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.WeekOfYear)
            {
                while (dt2.DayOfWeek != eFirstDayOfWeek)
                    dt2 = dt2.AddDays(-1);
                while (dt1.DayOfWeek != eFirstDayOfWeek)
                    dt1 = dt1.AddDays(-1);
                ts = dt2 - dt1;
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.Quarter)
            {
                double d1Quarter = GetQuarter(dt1.Month);
                double d2Quarter = GetQuarter(dt2.Month);
                double d1 = d2Quarter - d1Quarter;
                double d2 = (4 * (dt2.Year - dt1.Year));
                return Round(d1 + d2);
            }

            return 0;
        }

        private static long Round(double dVal)
        {
            if (dVal >= 0)
                return (long)Math.Floor(dVal);
            return (long)Math.Ceiling(dVal);
        }

        /// <summary>
        /// Returns datetime after adding given number of weekdays.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static DateTime AddWeekdays(DateTime start, int days)
        {
            // adjust for weekend start date
            if (start.DayOfWeek == DayOfWeek.Saturday)
            {
                start = start.AddDays(2);
            }
            if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                start = start.AddDays(1);
            }

            int remainder = days % 5;
            int weekendDays = (days / 5) * 2;


            DateTime end = start.AddDays(remainder);

            if (end.DayOfWeek == DayOfWeek.Saturday)
            {
                // add two days for landing on saturday 
                end = end.AddDays(2);
            }
            else if (end.DayOfWeek < start.DayOfWeek)
            {
                // add two days for rounding the weekend 
                end = end.AddDays(2);
            }
            // add the remaining days 
            return end.AddDays(days + weekendDays - remainder);
        }

    }
}
