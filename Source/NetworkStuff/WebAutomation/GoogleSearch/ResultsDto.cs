using System.Collections.Generic;
using System.Linq;
using System.Text;
using FackCheckThisBitch.Common;

namespace WebAutomation.GoogleSearch
{
    public class ResultsDto
    {
        public string Query;
        public Dictionary<string, (double howMany,string[] firstPageResults)> ResultsPerMonth;

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            StringBuilder chart = new StringBuilder();

            string prevKey = "";
            foreach (var key in ResultsPerMonth.Keys)
            {
                if (prevKey != "")
                {
                    result.AppendLine(Change(prevKey, ResultsPerMonth[prevKey].howMany, key, ResultsPerMonth[key].howMany));
                }

                chart.AppendLine($"{key}\t{ResultsPerMonth[key].howMany}");
                prevKey = key;
            }

            return chart.ToString();

        }

        public string[] AllResults
        {
            get
            {
                var result = new List<string>();
                foreach (var links in ResultsPerMonth.Values.Select(_ => _.firstPageResults))
                {
                    result.AddRange(links);
                }
                return result.ToArray();
            }
        }

        private string Change(string from, double fromValue, string to, double toValue)
        {
            var change = (toValue - fromValue) * 100f / fromValue;
            return $"{to} ({toValue}) change from {from} ({fromValue}): {change.GetSign()}{change:0.00} %";
        }

    }

}
