using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace FackCheckThisBitch.Common
{
    public static class LeetSpeak
    {
        public static readonly string[] WrongSpeak = new []
        {
            "world health organization",
            "Π.Ο.Υ.",
            "Παγκόσμιο",
            "WHO",
            "W.H.O.",
            "covid",
            "covid-19",
            "Κοβιντ",
            "vaccine",
            "εμβόλιο",
            "εμβολίου",
            "εμβολιαστεί",
            "εμβόλια",
            "εμβολίων",
            "εμβολιάσει",
            "εμβολίου",
            "vaccinate",
            "vaccinated",
            "pfizer",
            "Bill Gates",
            "Μπίλ Γκέιτς",
            "Μπίλ",
            "Bill & Melinda Gates Foundation",
            "Moderna",
            "pandemic",
            "πανδημία",
            "πανδημίες",
            "pandemics",
            "big pharma",
        };

        public static string ToLeetSpeak(this string input, Level level = Level.Minimum)
        {
            if (level == Level.None) return input;

            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (level >= Level.Maximum)
                {
                    if ((array[i] == 's' || array[i] == 'S') &&
                        !(i == 0 || array[i - 1] == ' ' || array[i - 1] == '\"' || array[i - 1] == '\''))
                    {
                        if (array[i] == 's')
                            array[i] = 'z';
                        else
                            array[i] = 'Z';
                    }
                }

                if (level >= Level.Minimum)
                {
                    switch (array[i])
                    {
                        case 'α':
                        case 'ά':
                            array[i] = 'a';
                            break;    
                        
                        case 'έ':
                        case 'ε':
                            array[i] = 'e';
                            break;   

                        case 'ό':
                        case 'ο':
                            array[i] = 'o';
                            break;  

                        case 'ύ':
                        case 'υ':
                            array[i] = 'u';
                            break;  

                        case 'ι':
                        case 'ί':
                            array[i] = 'i';
                            break;  


                        case 'a':
                            array[i] = '@';
                            break;
                        case 'C':
                        case 'c':
                            array[i] = '(';
                            break;

                        case 'o':
                        case 'O':
                            array[i] = '0';
                            break;
                        case 's':
                        case 'S':
                            array[i] = '5';
                            break;
                        case 'I':
                        case 'i':
                            array[i] = '1';
                            break;
                        case 'l':
                            array[i] = '|';
                            break;
                        case 't':
                            array[i] = '+';
                            break;
                    }
                }

                if (level >= Level.Medium)
                {
                    switch (array[i])
                    {
                        case 'e':
                            array[i] = '3';
                            break;

                        case 'T':
                            array[i] = '7';
                            break;

                        default:
                            break;
                    }
                }
            }

            var result = string.Join("", array);
            result = result.Replace(" ", "  ");
            return result;
        }

        public static string WrongSpeakToLeetSpeak(this string input, Level level = Level.Minimum)
        {
            if (level == Level.None) return input;

            foreach (var wrongWord in WrongSpeak)
            {
                var leetWrongWord = wrongWord.ToLeetSpeak(level);

                input = input.Replace(wrongWord, leetWrongWord, StringComparison.InvariantCultureIgnoreCase);
            }

            return input;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Level
    {
        [EnumMember(Value = "None")] None,
        [EnumMember(Value = "Minimum")] Minimum,
        [EnumMember(Value = "Medium")] Medium,
        [EnumMember(Value = "Maximum")] Maximum
    }
}