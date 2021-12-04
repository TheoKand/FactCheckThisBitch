using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FackCheckThisBitch.Common
{
    public static class Extensions
    {
        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static string TryGet(this IDictionary<string, string> input, string key)
        {
            if (input.ContainsKey(key))
            {
                return input[key];
            }
            else
            {
                return null;
            }
        }

        public static string HtmlDecode(this string input)
        {
            return System.Net.WebUtility.HtmlDecode(input);
        }

        public static string ToSanitizedString(this string input)
        {
            string result = "";
            if (input != null)
            {
                result = Regex.Replace(input, @"[^a-zA-Z0-9 -]", " ").Trim();
                result = result.Replace(" ", "-");
                result = result.Replace("--", "-");
                result = result.ToLower();
            }

            return result;
        }

        public static string[] CommaSeparatedListToArray(this string input)
        {
            return input.Split(",").Where(x => !x.IsEmpty()).Select(x => x.Trim()).ToArray();
        }

        public static string ToSimpleStringDate(this DateTime? input)
        {
            return input.HasValue ? input.Value.ToString("dd/MM/yyyy") : "";
        }

        public static DateTime? ToDate(this string date)
        {
            if (date.IsEmpty()) return null;
            var ukCulture = new CultureInfo("en-GB", false);
            var dateValue = DateTime.Parse(date, ukCulture);
            return dateValue;
        }

        public static IEnumerable<PropertyInfo> PropertiesNotFromInterface(this object instance)
        {
            var type = instance.GetType();
            var props = type.GetProperties();
            var implementedProps = type.GetInterfaces().SelectMany(i => i.GetProperties());
            var onlyInFoo = props.Select(prop => prop.Name).Except(implementedProps.Select(prop => prop.Name))
                .ToArray();
            var fooPropsFiltered = props.Where(x => onlyInFoo.Contains(x.Name));

            return fooPropsFiltered;
        }

        public static string RegExValidationPatternForType(this Type propType)
        {
            if (propType.Equals(typeof(DateTime)))
            {
                return
                    "^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[13-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$";
            }
            else if (propType.Equals(typeof(Int16)) || propType.Equals(typeof(Int32)) || propType.Equals(typeof(Int64)))
            {
                return "^-?[0-9]*[0-9,\\.]*$";
            }

            return null;
        }

        public static bool IsAlmostSameWith(this string phrase1, string phrase2)
        {
            phrase1 = phrase1.Trim();
            phrase2 = phrase2.Trim();

            string smallerPhrase = phrase1.Length < phrase2.Length ? phrase1 : phrase2;
            string largerPhrase =  phrase1.Length > phrase2.Length ? phrase1 : phrase2;

            return (largerPhrase.Contains(smallerPhrase, StringComparison.InvariantCultureIgnoreCase))
                || largerPhrase.ContainsTwoOrMoreWordsFrom(smallerPhrase);
        }

        public static bool ContainsTwoOrMoreWordsFrom(this string largePhrase, string smallPhrase)
        {
            var largePhraseWords = largePhrase.Split(" ");
            var smallPhraseWords = smallPhrase.Split(" ");
            var count = smallPhraseWords.Count(sw =>
                largePhraseWords.Any(lw=> String.Equals(lw,sw, StringComparison.InvariantCultureIgnoreCase)));
            return count >= 2;
        }

        public static bool HaveAtLeastOneCommonKeyword(this string[] keywords1, string[] keywords2)
        {
            var nonEmptyKeywords1 = keywords1.Where(k => !k.IsEmpty()).ToList();
            var nonEmptyKeywords2 = keywords2.Where(k => !k.IsEmpty()).ToList();

            if (nonEmptyKeywords1.Count == 0 || nonEmptyKeywords2.Count == 0) return false;

            foreach (string keyword in nonEmptyKeywords1)
            {
                string allOfKeywords2 = string.Join(",", nonEmptyKeywords2);
                if (allOfKeywords2.IsAlmostSameWith(keyword))
                {
                    return true;
                }
            }

            return false;
        }

        public static IList<T> Swap<T>(this IList<T> list, int zeroBasedIndexA, int zeroBasedIndexB)
        {
            (list[zeroBasedIndexA], list[zeroBasedIndexB]) = (list[zeroBasedIndexB], list[zeroBasedIndexA]);
            return list;
        }
    }
}