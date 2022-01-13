using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nager.Date;
using Nager.Date.Model;

namespace System
{
    public static class DateTimeExtensions
    {
        public static CultureInfo NorwegianCulture { get; } = new CultureInfo("nb-NO");

        /// <summary>
        /// Get name of day for the given <see cref="date"/> using Norwegian culture info.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDayName(this DateTime date) => NorwegianCulture.DateTimeFormat.GetDayName(date.DayOfWeek);

        public static int GetAge(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }
        public static int GetAge(this DateTime birthDate, DateTime toDate)
        {
            var today = toDate.Date;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }


        public static bool IsHoliday(this DateTime date) => DateSystem.IsPublicHoliday(date, CountryCode.NO);

        public static bool IsWorkingDay(this DateTime date) => (IsHoliday(date) == false && IsWeekend(date) == false);

        public static bool IsWeekend(this DateTime date) => DateSystem.IsWeekend(date, CountryCode.NO);

        public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-1 * diff).Date;
        }

        public static IEnumerable<DateTime> GenerateDatesInWeek(this DateTime weekDate) =>
         GenerateDatesFromDate(weekDate.StartOfWeek(), 6);

        public static IEnumerable<DateTime> GenerateDatesFromDate(this DateTime startIncluded, int numDays) =>
            GenerateDatesInPeriod(startIncluded, startIncluded.AddDays(numDays));

        public static IEnumerable<DateTime> GenerateDatesInPeriod(DateTime startIncluded, DateTime endIncluded) =>
            Enumerable
                .Range(0, int.MaxValue)
                .Select(index => startIncluded.AddDays(index))
                .TakeWhile(date => date <= endIncluded)
                .ToList();


        public static IEnumerable<PublicHoliday> GetHolidays(int year) => DateSystem.GetPublicHolidays(DateTime.Parse("01.01." + year), DateTime.Parse("31.12." + year), CountryCode.NO);

        public static string ToDbDateString(this DateTime date) => date.ToString("yyyyMMdd");
        public static string ToDbDateTimeString(this DateTime date) => date.ToString("yyyyMMddHHmm");


        public enum DateFormat
        {
            /// <summary>
            /// Datoformat yyyyMMdd
            /// </summary>
            yyyyMMdd,
            /// <summary>
            /// Datoformat yyyyMMddHHmm
            /// </summary>
            yyyyMMddHHmm,
            /// <summary>
            /// Datoformat yyyyMMddHHmmss
            /// </summary>
            yyyyMMddHHmmss,
            /// <summary>
            /// Datoformat ddMMyy
            /// </summary>
            ddMMyy,
            /// <summary>
            /// Datoformat ddMMyyyy
            /// </summary>
            ddMMyyyy,
            /// <summary>
            /// Datoformat ddMMyyyyHHmm
            /// </summary>
            ddMMyyyyHHmm,
            /// <summary>
            /// Datoformat dd.MM.yyyy
            /// </summary>
            dd_MM_yyyy,
            dd_MM_yy,
            /// <summary>
            /// Datoformat dd.MM.yyyy HH:mm
            /// </summary>
            dd_MM_yyyy_HH_mm,
            /// <summary>
            /// Datoformat dd.MM.yyyy HH:mm:ss
            /// </summary>
            dd_MM_yyyy_HH_mm_ss,
            /// <summary>
            /// Datoformat HH:mm
            /// </summary>
            HH_mm,
            /// <summary>
            /// Datoformat HH:mm:ss
            /// </summary>
            HH_mm_ss,
            /// <summary>
            /// Datoformat HHmm
            /// </summary>
            HHmm,
            /// <summary>
            /// Datoformat HHmmss
            /// </summary>
            HHmmss,
            /// <summary>
            /// Datoformat MM/dd/yyyy HH:mm:ss
            /// </summary>
            MM_dd_yyyy_HH_mm_ss

        }

        private static CultureInfo formatProvider = new CultureInfo("nb-NO", false);

        public static DateTime StringToDateTime(string s, DateFormat innFormat)
        {            
            DateTime value = new DateTime();
            try
            {
                if (!string.IsNullOrEmpty(s)) //For å hindre at tomme strenger fører til feil
                {
                    //20170627 KBO: For å unngå Windows10 problem med bytte av klokkeskilletegn til . istedenfor :
                    s = s.Strip(".").Strip(":").Strip(" ").Strip("/");

                    if (s.ContainsOnlyGivenChar('9'))
                    {
                        value = DateTime.MinValue;
                    }
                    else
                    {
                        switch (innFormat)
                        {
                            case DateFormat.dd_MM_yyyy:
                                value = DateTime.ParseExact(s, "ddMMyyyy", formatProvider, DateTimeStyles.None);
                                break;
                            case DateFormat.dd_MM_yy:
                                value = DateTime.ParseExact(s, "ddMMyy", formatProvider, DateTimeStyles.None);
                                break;
                            case DateFormat.dd_MM_yyyy_HH_mm:
                                value = DateTime.ParseExact(s, "ddMMyyyyHHmm", formatProvider, DateTimeStyles.None);
                                break;
                            case DateFormat.dd_MM_yyyy_HH_mm_ss:
                                value = DateTime.ParseExact(s, "ddMMyyyyHHmmss", formatProvider, DateTimeStyles.None);
                                break;
                            case DateFormat.HH_mm:
                                value = DateTime.ParseExact(s, "HHmm", formatProvider, DateTimeStyles.None);
                                break;
                            case DateFormat.HH_mm_ss:
                                value = DateTime.ParseExact(s, "HHmmss", formatProvider, DateTimeStyles.None);
                                break;
                            case DateFormat.MM_dd_yyyy_HH_mm_ss:
                                value = DateTime.ParseExact(s, "MMddyyyyHHmmss", formatProvider, DateTimeStyles.None);
                                break;
                            default:
                                value = DateTime.ParseExact(s, innFormat.ToString(), formatProvider, DateTimeStyles.None);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return value;
        }
    }
}
