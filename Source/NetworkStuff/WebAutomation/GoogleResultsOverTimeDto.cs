using System.Collections.Generic;
using System.Text;

namespace WebAutomation
{
    public class GoogleResultsAggregateDto
    {
        public string Query;
        public Dictionary<int, double> ResultsPerYear;
        public Dictionary<string, double> ResultsPerMonth;

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            StringBuilder chart = new StringBuilder();

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

            return chart.ToString();

        }

        private string Change(string from, double fromValue, string to, double toValue)
        {
            var change = (toValue - fromValue) * 100f / fromValue;
            return $"{to} ({toValue}) change from {from} ({fromValue}): {change.GetSign()}{change:0.00} %";
        }

    }

}
