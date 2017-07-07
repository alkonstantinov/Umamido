using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Umamido.Common.Tools
{
    public class Tools
    {
        public static String StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<[/a-zA-Z]+[^>]*>", "");
        }

        public static void GetDay(DateTime date, ref DateTime d1, ref DateTime d2)
        {
            d1 = date;
            TimeSpan ts = new TimeSpan(0, 0, 0);
            d1 = d1.Date + ts;
            d2 = date;
            ts = new TimeSpan(23, 59, 59);
            d2 = d2.Date + ts;
            
        }
        public static void GetWeek(DateTime date, ref DateTime d1, ref DateTime d2)
        {
            d1 = date;
            TimeSpan ts = new TimeSpan(0, 0, 0);
            d1 = d1.Date + ts;
            while (d1.DayOfWeek != DayOfWeek.Monday)
                d1 = d1.AddDays(-1);
            d2 = date;
            ts = new TimeSpan(23, 59, 59);
            d2 = d2.Date + ts;
            while (d2.DayOfWeek != DayOfWeek.Sunday)
                d2 = d2.AddDays(1);

        }

        public static void GetYear(DateTime date, ref DateTime d1, ref DateTime d2)
        {
            d1 = new DateTime(date.Year, date.Month, date.Day);
            TimeSpan ts = new TimeSpan(0, 0, 0);
            d1 = d1.Date + ts;
            d2 = new DateTime(date.Year + 1, date.Month, date.Day);
            d2 = d2.AddMinutes(-1);

        }

        public static void GetMonth(DateTime date, ref DateTime d1, ref DateTime d2)
        {
            d1 = new DateTime(date.Year, date.Month, 1);
            TimeSpan ts = new TimeSpan(0, 0, 0);
            d1 = d1.Date + ts;
            d2 = d1.AddMonths(1);
            d2 = d2.AddMinutes(-1);

        }

        public static void Get2Years(DateTime date, ref DateTime d1, ref DateTime d2)
        {
            d1 = new DateTime(date.Year - 1, date.Month, date.Day);
            TimeSpan ts = new TimeSpan(0, 0, 0);
            d1 = d1.Date + ts;
            d2 = new DateTime(date.Year + 1, date.Month, date.Day);
            d2 = d2.AddMinutes(-1);

        }
    }
}
