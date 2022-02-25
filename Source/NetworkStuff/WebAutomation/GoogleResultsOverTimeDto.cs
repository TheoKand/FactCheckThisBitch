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
        public Dictionary<int, double> ResultsPerYear;
        public Dictionary<string, double> ResultsPerMonth;

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
            StringBuilder chart = new StringBuilder();

            if (ResultsPerYear != null)
            {
                result.AppendLine(Change("2016",ResultsPerYear[2016],"2017",ResultsPerYear[2017]));
                result.AppendLine(Change("2017", ResultsPerYear[2017], "2018", ResultsPerYear[2018]));
                result.AppendLine(Change("2018", ResultsPerYear[2018], "2019", ResultsPerYear[2019]));
                result.AppendLine(Change("2019", ResultsPerYear[2019], "2020", ResultsPerYear[2020]));
                result.AppendLine(Change("2020", ResultsPerYear[2020], "2021", ResultsPerYear[2021]));

                var change2022 = LastYearProjectedChange;
                result.AppendLine($"2022 ({ResultsPerYear[2022]}) projected change from previous year ({ResultsPerYear[2021]}): {change2022.GetSign()}{change2022:0.00} %");

            } else if (ResultsPerMonth != null)
            {
                string prevKey = "";
                foreach (var key in ResultsPerMonth.Keys)
                {
                    if (prevKey != "")
                    {
                        result.AppendLine(Change(prevKey, ResultsPerMonth[prevKey], key, ResultsPerMonth[key]));
                    }

                    chart.AppendLine($"{key}\t{ResultsPerMonth[key]}");
                    prevKey = key;
                }
            }

            var chartCopy = chart.ToString();

            //return result.ToString();
            return chartCopy;

        }

        private string Change(string from,double fromValue, string to, double toValue)
        {
            var change = (toValue - fromValue) * 100f / fromValue;
            return $"{to} ({toValue}) change from {from} ({fromValue}): {change.GetSign()}{change:0.00} %";
        }

        public string YearChangeFromLastYear(Dictionary<int, double> resultsPerYear, int year)
        {
            var change = (resultsPerYear[year] - resultsPerYear[year-1]) * 100f / resultsPerYear[year-1];
            return $"{year} ({resultsPerYear[year]}) change from {year-1} ({resultsPerYear[year - 1]}): {change.GetSign()}{change:0.00} %";
        }
    }

}
