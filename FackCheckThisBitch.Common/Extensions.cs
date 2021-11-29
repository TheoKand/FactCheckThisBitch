using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace FackCheckThisBitch.Common
{
    public static class Extensions
    {
        public static string ToSanitizedString(this string input)
        {
            string result="";
            if (input != null)
            {
                result = Regex.Replace(input, @"[^a-zA-Z0-9 -]", " ").Trim();
                result = result.Replace(" ", "-");
                result = result.Replace("--", "-");
                result = result.ToLower();
            }
            return result;
        }

        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static string[] CommaSeparatedListToArray(this string input)
        {
            return input.Split(",").Select(x=>x.Trim()).ToArray();
        }

        public static string ToSimpleStringDate(this DateTime? input)
        {
            return input.HasValue?input.Value.ToString("dd/MM/yyyy"):"";
        }

        public static DateTime? ToDate(this string date)
        {
            if (date == null) return null;
            var ukCulture = new CultureInfo("en-GB",false);
            var dateValue = DateTime.Parse(date, ukCulture);
            return dateValue;
        }


    }
}
