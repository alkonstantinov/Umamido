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
        static public int getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return (int)Math.Floor(d);
        }

        static public double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }



        public static String StripHtmlTags(string input)
        {
            return input == null ? null : Regex.Replace(input, "<[/a-zA-Z]+[^>]*>", "");
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

        private static string convert(string in_s, string Gender)
        {
            string[] stotici = new string[] { "", "сто ", "двеста ", "триста " };
            string[] desetici = new string[] { "десет ", "единадесет ", "дванадесет " };
            string[] edinici = new string[] { "", "едно", "две", "три", "четири", "пет", "шест", "седем", "осем", "девет" };
            if (Gender == "male")
            {
                edinici[1] = "един"; edinici[2] = "два";
            }
            else if (Gender == "female")
            {
                edinici[1] = "една";
            }

            var stot = int.Parse(in_s.Substring(0, 1));
            var deset = int.Parse(in_s.Substring(1, 1));
            var ed = int.Parse(in_s.Substring(2, 1));

            var Result = "";
            if (stot > 0 && stot <= 3) Result = stotici[stot];
            else if (stot > 3) Result = edinici[stot] + "стотин ";

            if (deset == 1)
            {
                if (Result != "") Result += "и ";
                if (ed >= 0 && ed <= 2) Result += desetici[ed];
                else Result += edinici[ed] + "надесет ";
            }
            else
            {
                if (deset > 1)
                {
                    if (Result != "" && ed == 0) Result += "и ";
                    if (deset == 2) Result += "двадесет ";
                    else Result += edinici[deset] + "десет ";
                }

                if (ed != 0)
                {
                    if (Result != "") Result += "и ";
                    Result += edinici[ed] + " ";
                }
            }

            return Result;
        }

        private static string toSlovom(int num, string Gender)
        {
            string[] TempGender = new string[] { Gender, "female", "male", "male" };
            string[,] mantisi = new string[,]{{"","хиляди ","милиона ","милиарда "},
                 {"","хилядa ","милион ","милиард "}};
            string ss;
            int pl;
            int in_pos = 0;
            string Result = "";

            string s = num.ToString();
            int len = (s.Length + 2) / 3;
            while (s.Length < 3 * len)
                s = "0" + s;

            for (int j = len - 1; j >= 0; j--)
            {
                string gr = s.Substring((len - 1 - j) * 3, 3);
                if (gr != "000")
                {
                    if (j == 1 && gr == "001")
                    {  // хиляда
                        ss = "";
                    }
                    else
                    {
                        ss = convert(gr, TempGender[j]);
                    }
                    if (gr == "001")
                        pl = 1;
                    else pl = 0;
                    ss += mantisi[pl, j];

                    if (ss.IndexOf(" и ") == -1) in_pos = Result.Length; else in_pos = 0;
                    Result += ss;
                }
            }
            if (in_pos != 0)
            {
                Result = Result.Substring(0, in_pos) + "и " + Result.Substring(in_pos);
            }

            return Result;
        }

        public static string toSlovomLeva(decimal number)
        {
            string res = "";
            if (number < 0)
                res = "минус ";
            number = Math.Abs(number);
            int lv = Convert.ToInt32(Math.Truncate(number));

            int st = Convert.ToInt32(Math.Round(number * 100, 0) % 100);


            if (lv != 0)
            {
                res += toSlovom(lv, "male") + "лв.";
            }
            if (st != 0)
            {
                if (res != "") res += " и ";
                res += toSlovom(st, "female") + "ст.";
            }
            if (res == "") res = "нула лв.";

            return res;
        }
    }
}
