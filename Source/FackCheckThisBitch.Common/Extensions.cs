using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace FackCheckThisBitch.Common
{
    public static class Extensions
    {
        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotEmpty(this string input)
        {
            return !input.IsEmpty();
        }

        public static string ValueOrNull(this string input)
        {
            return input.IsEmpty() ? null : input;
        }

        public static string Limit(this string input, int max,string appendIfCut = null)
        {
            var result = "";
            if (input?.Length > max)
            {
                if (appendIfCut.IsNotEmpty())
                {
                    result = input.Substring(0, max - appendIfCut.Length) + appendIfCut;
                }
                else
                {
                    result = input.Substring(0, max);
                }
                

            }
            else
            {
                result= input;
            }

            return result;

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

        public static string Sanitize(this string input)
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

        public static bool ContainsOneOfThese(this string input, IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                if (input.Contains(word, StringComparison.InvariantCultureIgnoreCase))
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

        public static string StripNarrationTags(this string input)
        {
            return input.Replace("[startSpeech v=Loud startSpeech]", "")
                .Replace("[endSpeech]", "")
                .Replace("[sPause sec=1 ePause]", "");
        }
        public static string StripLinefeeds(this string input, string replaceWith="")
        {
            return input.Replace("\r\n", replaceWith)
                .Replace("\r", replaceWith)
                .Replace("\n", replaceWith)
                .Replace(Environment.NewLine, replaceWith);
        }

        public static string SecondsToTimeline(this double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            string str = time.ToString(@"mm\:ss");
            return str;
        }

        public static double CalculateNarrationDuration(this string input)
        {
            //characters / seconds = ratio
            //seconds = characters / ratio

            //4165  / 264 = 15.77651515151515

            if (!string.IsNullOrEmpty(input))
            {
                var duration = input.Length / 15.77651515151515;
                return duration;

            }

            return 0;



        }

        public static string DictionaryToString(this IDictionary<string, string> input)
        {
            if (input == null) return null;
            var results = new StringBuilder();
            foreach (var key in input.Keys)
            {
                if (!input[key].IsEmpty())
                {
                    results.AppendLine($"{key}:{input[key]}");
                }
            }
            return results.ToString();
        }

        public static void DelayRandom(int fromMilliseconds = 500, int toMilliseconds = 2000)
        {
            var milliSeconds = new Random().Next(fromMilliseconds, toMilliseconds);
            Thread.Sleep(milliSeconds);
        }

        public static string GetSign(this double input)
        {
            return input > 0 ? "+" : "";
        }

        public static TimeSpan ParseTimeSpan(this string input)
        {
            try
            {
                var min = Convert.ToInt16(input.Split(":")[0]);
                var sec = Convert.ToInt16(input.Split(":")[1]);
                var result = TimeSpan.FromSeconds(min * 60 + sec);
                return result;
            }
            catch
            {
                return TimeSpan.FromSeconds(0);
            }

        }

        public static T CloneObject<T>(this T input)
        {
            var serialized = JsonConvert.SerializeObject(input);
            var cloned = JsonConvert.DeserializeObject<T>(serialized);
            return cloned;
        }

        public static void CopyTo(this DirectoryInfo diSource, string to)
        {
            var diTarget = new DirectoryInfo(to);

            CopyAll(diSource, diTarget);

            void CopyAll(DirectoryInfo source, DirectoryInfo target)
            {
                Directory.CreateDirectory(target.FullName);

                // Copy each file into the new directory.
                foreach (FileInfo fi in source.GetFiles())
                {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }

                // Copy each subdirectory using recursion.
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
        }

        public static string SanitizeNarration(this string narration)
        {
            narration = narration.Replace("(pictured)", " ");
            narration = narration.Replace(".com", " dot com ");
            narration = narration.Replace("sh*t", "beep");
            narration = narration.Replace("shit", "beep");
            narration = narration.Replace("Sen.", "Senator ");
            narration = narration.Replace("a**", "beep");
            narration = narration.Replace("ass", "beep");
            narration = narration.Replace("E! News", "E News");

            if (narration.Contains("*"))
            {
                throw new Exception($"Invalid character * in narration '{narration}'");
            }

            return narration;
        }



    }
}