using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using OpenQA.Selenium.DevTools.V85.Input;

namespace WebAutomation
{
    public class GoogleResultsAggregateDto
    {
        public string Query;
        public bool OnlyNews;
        public Dictionary<int, double> ResultsPerYear;

        public double LastYearProjectedChange
        {
            get
            {
                if (!ResultsPerYear.ContainsKey(2022) || !ResultsPerYear.ContainsKey(2021))
                {
                    return 0;
                }
                var projection = ResultsPerYear[2022] * (365f / DateTime.Today.DayOfYear);
                var change2022 = (projection - ResultsPerYear[2021]) * 100f / ResultsPerYear[2021];
                return change2022;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(YearChangeFromLastYear(ResultsPerYear, 2017));
            result.AppendLine(YearChangeFromLastYear(ResultsPerYear, 2018));
            result.AppendLine(YearChangeFromLastYear(ResultsPerYear,2019));
            result.AppendLine(YearChangeFromLastYear(ResultsPerYear, 2020));
            result.AppendLine(YearChangeFromLastYear(ResultsPerYear, 2021));

            var change2022 = LastYearProjectedChange;
            result.AppendLine($"2022 projected change from previous year: {change2022.GetSign()}{change2022:0.00} %");

            return result.ToString();

        }

        public string YearChangeFromLastYear(Dictionary<int, double> resultsPerYear, int year)
        {
            var change = (resultsPerYear[year] - resultsPerYear[year-1]) * 100f / resultsPerYear[year-1];
            return $"{year} change from {year-1}: {change.GetSign()}{change:0.00} %";
        }
    }

}
