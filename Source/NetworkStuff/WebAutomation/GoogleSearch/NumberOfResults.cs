using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FackCheckThisBitch.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V85.Memory;
using SeleniumExtras.WaitHelpers;

namespace WebAutomation.GoogleSearch
{
    public class NumberOfResults
    {
        private IWebDriver _driver;

        private string _chromedriverLocation =
            "C:\\Users\\theok\\Repos\\FactCheckThisBitch\\Source\\NetworkStuff\\WebAutomation";

        private string _dummyPhrase = "vaccination near me";

        private string _testPhrases =
            "died suddenly,dies of heart attack,his unexpected death,his sudden death,her sudden death,her unexpected death,died of cardiac arrest,sudden passing,suspected heart attack";

        private string _controlPhrases =
            "resignes,arrives,departs,time will tell,first glance,critics say,the question,make no mistake,blind eye";

        private string _sites =
            "mylondon.news,dailymail.co.uk,birminghammail.co.uk,liverpoolecho.co.uk,dailyrecord.co.uk,mirror.co.uk,irishpost.com,scotsman.com,kentlive.news,manchestereveningnews.co.uk";

        private string BuildQuery(string query)
        {
            var phrasesList = string.Join(" OR ",
                query.Split(',')
                    .Select(_ => $"\"{_}\""));
            var siteList = string.Join(" OR ",
                _sites.Split(',')
                    .Select(_ => $"site:{_}"));
            var result = $"({phrasesList}) and ({siteList}) ";
            return result;
        }

        private string BuildQueryV2(string query)
        {
            var phrasesList = string.Join(" OR ",
                query.Split(',')
                    .Select(_ => $"intitle:\"{_}\""));
            var siteList = string.Join(" OR ",
                _sites.Split(',')
                    .Select(_ => $"site:{_}"));
            var result = $"({phrasesList}) and ({siteList}) ";
            return result;
        }

        public NumberOfResults()
        {
            _driver = new ChromeDriver(_chromedriverLocation);
            _driver.Url = "https://www.google.com";
            _driver.Manage()
                .Window.Maximize();
            FackCheckThisBitch.Common.Extensions.DelayRandom();
            _driver.Url = $"https://www.google.com/search?q={_dummyPhrase}";

            //click agree button
            try
            {
                _driver.FindWaitElementForClick("//*[contains(text(), 'I agree')]",
                        10)
                    ?.Click();
            }
            catch
            {
            }
        }

        private async Task GetMetadata(string[] links)
        {
            foreach (var url in links)
            {
                var onlineArticleParser = new ArticleMetadataParser($"https://{url}");
                try
                {
                    var metaData = await onlineArticleParser.Download();
                    var metaDataText = metaData.DictionaryToString();
                    if (!metaDataText.IsEmpty())
                    {
                        await TestContext.Out.WriteLineAsync(url);
                        await TestContext.Out.WriteLineAsync(metaDataText);
                    }
                }
                catch (Exception ex)
                {
                    await TestContext.Out.WriteLineAsync($"Error parsing metadata {url}: {ex.Message}");
                }
            }

        }
        [Test]
        public void TestPhrases()
        {
            var result = GetGoogleResultsPerMonth(_testPhrases);
            TestContext.Out.WriteLine($"{result}");

            GetMetadata(result.AllResults).Wait();
        }

        [Test]
        public void ControlPhrases()
        {
            var result = GetGoogleResultsPerMonth(_controlPhrases);
            TestContext.Out.WriteLine($"{result}");
            TestContext.Out.WriteLine(
                $"{string.Join(Environment.NewLine, result.ResultsPerMonth.Values.Select(_ => _.firstPageResults))}");
        }

        private ResultsDto GetGoogleResultsPerMonth(string query)
        {
            var result = new ResultsDto
            {
                Query = query,
                ResultsPerMonth = new Dictionary<string, (double, string[])>()
            };
            var startYear = 2021; //2018;
            var startDate = new DateTime(startYear,
                1,
                1);
            var endDate = DateTime.Today;
            var date = startDate;
            while (date < endDate)
            {
                var toDate = date.AddMonths(1)
                    .AddDays(-1);
                var results = QueryGoogleGetResultsForTimePeriod(query,
                    date.ToString("MM/dd/yyyy"),
                    toDate.ToString("MM/dd/yyyy"));
                result.ResultsPerMonth.Add(date.ToString("MM-yyyy"),
                    results);
                date = date.AddMonths(1);
            }

            return result;
        }

        private (double, string[]) QueryGoogleGetResultsForTimePeriod(string searchPhrases,
            string fromDate = null,
            string toDate = null)
        {
            var query = BuildQueryV2(searchPhrases);
            _driver.Url = $"https://www.google.com/search?q={query}&tbs=cdr:1,cd_min:{fromDate},cd_max:{toDate}";
            FackCheckThisBitch.Common.Extensions.DelayRandom(500,
                1000);

            //var searchBox = _driver.FindWaitElementForClick("//input[contains(@aria-label, 'Search')]");
            //searchBox.SendKeys(query);
            //searchBox.SendKeys( Keys.Enter);
            //Extensions.DelayRandom(500, 1000);

            //click Tools button
            _driver.FindWaitElementForClick("//div[contains(text(), 'Tools')]")
                .Click();
            double howManyResults = 0;
            string[] results = new string[0];
            try
            {
                var howManyResultsElement = _driver.FindElement(By.Id("result-stats"));
                bool hasResults = _driver.GetWait()
                    .Until(ExpectedConditions.TextToBePresentInElement(howManyResultsElement,
                        "result"));
                if (hasResults)
                {
                    var resultText = howManyResultsElement.Text;
                    howManyResults = ExtractResultCount(resultText);

                    //get all links from first page
                    string searchResults = _driver.FindElement(By.Id("search"))
                        .GetAttribute("innerHTML");
                    results = ExtractArticleLinks(searchResults);
                }
            }
            catch
            {
            }

            return (howManyResults, results.ToArray());
        }

        private double ExtractResultCount(string text)
        {
            var matches = Regex.Matches(text,
                "\\s*([^\\s]+)\\s+result");
            if (!matches.Any() ||
                matches.First()
                    .Groups.Count <
                2)
                return 0;
            var numOfResults = double.Parse(matches.First()
                .Groups[1]
                .Value);
            return numOfResults;
        }

        private string[] ExtractArticleLinks(string html)
        {
            var results = new List<string>();
            var linkMatches = Regex.Matches(html,
                "<a href=\"https:\\/\\/([^\"]+)\"");
            foreach (Match match in linkMatches)
            {
                var url = match.Groups[1]
                    .Value;
                results.Add(url);
            }

            return results.ToArray();
        }
    }
}