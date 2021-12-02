using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

        public static string[] CommaSeparatedListToArray(this string input)
        {
            return input.Split(",").Where(x=>!string.IsNullOrWhiteSpace(x)).Select(x=>x.Trim()).ToArray();
        }

        public static string ToSimpleStringDate(this DateTime? input)
        {
            return input.HasValue?input.Value.ToString("dd/MM/yyyy"):"";
        }

        public static DateTime? ToDate(this string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return null;
            var ukCulture = new CultureInfo("en-GB",false);
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
            {

            }

            return null;
        }


    }
}
