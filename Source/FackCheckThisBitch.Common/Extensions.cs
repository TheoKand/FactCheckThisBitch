﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace FackCheckThisBitch.Common
{
    public static class Extensions
    {
        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static string ValueOrNull(this string input)
        {
            return input.IsEmpty() ? null : input;
        }

        public static string Limit(this string input, int max)
        {
            if (input.Length > max)
            {
                return input.Substring(0, max);
            }
            else
            {
                return input;
            }

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
                result = Regex.Replace(input, @"[^άέύίόώα-ωΑ-Ωa-zA-Z0-9 -]", " ").Trim();
                result = result.Replace(" ", "-");
                result = result.Replace("--", "-");
                result = result.ToLower();
            }

            return result;
        }

        public static string[] CommaSeparatedListToArray(this string input)
        {
            if (input.IsEmpty()) return null;
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
            var onlyInFoo = props.Select(prop => prop.Name).Except(implementedProps.Select(prop => prop.Name)).ToArray();
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

        public static bool IsValidUrl(this string input)
        {
            var pattern =
                "^((http|ftp|https|www)://).([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?$";
            return Regex.IsMatch(input, pattern);
        }

        private static bool IsAlmostSameWith(this string phrase1, string phrase2)
        {
            phrase1 = phrase1.Trim();
            phrase2 = phrase2.Trim();

            string smallerPhrase = phrase1.Length <= phrase2.Length ? phrase1 : phrase2;
            string largerPhrase = phrase1.Length > phrase2.Length ? phrase1 : phrase2;

            return (largerPhrase.Contains(smallerPhrase, StringComparison.InvariantCultureIgnoreCase)) ||
                   largerPhrase.ContainsOneOrMoreWordsFrom(smallerPhrase);
        }

        private static bool ContainsOneOrMoreWordsFrom(this string largePhrase, string smallPhrase)
        {
            var largePhraseWords = largePhrase.Split(" ").Where(w => w.Length > 2).ToArray();
            var smallPhraseWords = smallPhrase.Split(" ").Where(w => w.Length > 2).ToArray();

            int minimumCommonWords = largePhraseWords.Count() == 2 ? 1 : 2;

            var count = smallPhraseWords.Count(sw =>
                largePhraseWords.Any(lw => string.Equals(lw, sw, StringComparison.InvariantCultureIgnoreCase)));
            return count >= minimumCommonWords;
        }

        public static bool HaveAtLeastOneCommonKeyword(this IEnumerable<string> keywords1, IEnumerable<string> keywords2)
        {
            if (!keywords1.Any() || !keywords2.Any()) return false;
            var nonEmptyKeywords1 = keywords1.Where(k => !k.IsEmpty()).ToList();
            var nonEmptyKeywords2 = keywords2.Where(k => !k.IsEmpty()).ToList();

            if (nonEmptyKeywords1.Count == 0 || nonEmptyKeywords2.Count == 0) return false;

            foreach (string keyword1 in nonEmptyKeywords1)
            {
                foreach (string keyword2 in nonEmptyKeywords2)
                {
                    if (keyword2.IsAlmostSameWith(keyword1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static IList<T> Swap<T>(this IList<T> list, int zeroBasedIndexA, int zeroBasedIndexB)
        {
            (list[zeroBasedIndexA], list[zeroBasedIndexB]) = (list[zeroBasedIndexB], list[zeroBasedIndexA]);
            return list;
        }

        public static string Obfuscate(this string input)
        {
            IDictionary<string, string> keywords = new Dictionary<string, string>()
            {
                {"covid", "c_v_d"},
                {"vaccine", "va_c_1n_e"},
                {"Pfizer", "Faizzer"},

            };

            foreach (string key in keywords.Keys)
            {
                input = input.Replace(key, keywords[key], StringComparison.InvariantCultureIgnoreCase);
            }

            return input;
        }

        public static int ToSpeechDuration(this string input)
        {
            //characters / seconds = ratio
            //seconds = characters / ratio

            //283 / 18    15.72
            //793 / 52.9   14.9
            //3744     /  236 sec    = 15.86
            //avg = 15.35

            if (!string.IsNullOrEmpty(input))
            {
                input = input.Trim();
                input = input.Replace(",", ",,,");
                input = input.Replace(".", "...");
            }
            return !string.IsNullOrEmpty(input) ? (int)Math.Ceiling(input.Length / 15.86) : 0;

            
        }

    }
}