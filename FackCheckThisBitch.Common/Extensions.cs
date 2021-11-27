﻿using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace FackCheckThisBitch.Common
{
    public static class Extensions
    {
        public static string ToSanitizedString(this string input)
        {
            var result = Regex.Replace(input, @"[^a-zA-Z0-9 -]", " ").Trim();
            result = result.Replace(" ", "-");
            result = result.ToLower();
            return result;
        }

        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

    }
}
