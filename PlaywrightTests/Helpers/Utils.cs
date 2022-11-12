using PlaywrightTests.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace PlaywrightTests.Helpers
{
    public static class Utils
    {
        public static class TableExtensions
        {
            public static Dictionary<string, string> ToDictionary(Table table)
            {
                var dictionary = new Dictionary<string, string>();
                foreach (var row in table.Rows)
                {
                    dictionary.Add(row[0], row[1]);
                }
                return dictionary;
            }
        }

        public static string GetLocatorByLabelName(string labelName)
        {
            return AppSettingsParamReplace(AppSettings.Locators.FillInputUnderLabel, labelName);
        }

        public static string GetLocatorByGuestType(string guestType)
        {
            return AppSettingsParamReplace(AppSettings.Locators.AddGuestsBasedOnGuestType, guestType);
        }

        public static string GetLocatorByDateFormat(string date)
        {
            return AppSettingsParamReplace(AppSettings.Locators.CalendarDaySelector, date);
        }

        public static string GetLocatorByFilterArea(string area)
        {
            return AppSettingsParamReplace(AppSettings.Locators.FilterValues, area);
        }

        public static string AppSettingsParamReplace(string locator, string varInLocator)
        {
            return locator.Replace("{{param}}", varInLocator);
        }

        public static string ConstructDateStringFormat(DateTime checkIn, DateTime checkOut)
        {
            var checkInMonthAbbreviation = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(checkIn.Month);
            var checkOutMonthAbbreviation = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(checkOut.Month);

            if (checkInMonthAbbreviation == checkOutMonthAbbreviation)
            {
                return checkInMonthAbbreviation + " " + checkIn.Day + " – " + checkOut.Day;
            }
            else
            {
                return checkInMonthAbbreviation + " " + checkIn.Day + " – " + checkOutMonthAbbreviation + " " + checkOut.Day;
            }
        }

        public static List<string> GuestsProcessor(string guests)
        {
            List<string> guestsAsList = new List<string>();

            Regex regex = new Regex("\"(.*?)\"");
            var matches = regex.Matches(guests);
            foreach (Match match in matches)
            {
                string[] _guests = match.Groups[1].Value.Split(' ');
                for (int i = 0; i < Int32.Parse(_guests.First()); i++)
                {
                    guestsAsList.Add(_guests.Last());
                }
            }

            return guestsAsList;
        }

        public static double AddDaysProcessor(string date)
        {
            Regex regex = new Regex("\"(.*?)\"");
            var matches = regex.Matches(date);
            return Double.Parse(matches[0].Value.Replace("\"", ""));
        }

        public static string GetBackgroundColorFromCssStyle(string cssStyle)
        {
            int trimFrom = cssStyle.IndexOf("background-color: ") + "background-color: ".Length;
            int trimTo = cssStyle.IndexOf(";");
            string result = cssStyle.Substring(trimFrom, trimTo - trimFrom);

            return GetColorFromRootVar(result);
        }

        public static string GetColorFromRootVar(string var)
        {
            switch (var)
            {
                case "var(--f-mkcy-f)":
                    return "#FFFFFF";
                case "var(--f-k-smk-x)":
                    return "#222222";
                default: return "Color not found";
            }
        }
    }
}
