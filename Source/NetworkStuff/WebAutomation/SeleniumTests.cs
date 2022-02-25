using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebAutomation
{

    public class SeleniumTests
    {
        private IWebDriver _driver;

        private string _chromedriverLocation =
            "C:\\Users\\theok\\Repos\\FactCheckThisBitch\\Source\\NetworkStuff\\WebAutomation";

        private string _dummyPhrase = "vaccination near me";
        private string _testPhrases = "died suddenly,dies of heart attack,his unexpected death,his sudden death,her sudden death,her unexpected death,died of cardiac arrest";
        private string _controlPhrases = "first glance,critics say,the question,needless to say,make no mistake,time will tell,blind eye,end of the day";
        private string _sites = "bbc.com,dailymail.co.uk,msn.com,foxnews.com,abcnews.go.com,nbcnews.com,cnn.com,news.yahoo.com,cbsnews.com,usnews.com";

        private string BuildQuery(string query)
        {
            var phrasesList = string.Join(" OR ", query.Split(',').Select(_ => $"\"{_}\""));

            var siteList = string.Join(" OR ", _sites.Split(',').Select(_ => $"site:{_}"));
            var result = $"({phrasesList}) and ({siteList}) ";
            return result;
        }

        public SeleniumTests()
        {
            _driver = new ChromeDriver(_chromedriverLocation);
            _driver.Url = "https://www.google.com";
            _driver.Manage().Window.Maximize();
            Extensions.DelayRandom();
            _driver.Url = $"https://www.google.com/search?q={_dummyPhrase}";
            
            //click agree button
            try
            {
                _driver.FindWaitElementForClick("//*[contains(text(), 'I agree')]", 10)?.Click();
            }
            catch { }
           
        }

        [Test]
        public void TestPhrases()
        {
            var result = GetGoogleResultsMonthlyAggregate(_testPhrases);
            TestContext.Out.WriteLine($"{result}");
        }

        [Test]
        public void ControlPhrases()
        {
            var result = GetGoogleResultsMonthlyAggregate(_controlPhrases);
            TestContext.Out.WriteLine($"{result}");
        }

        private GoogleResultsAggregateDto GetGoogleResultsMonthlyAggregate(string query)
        {
            var result = new GoogleResultsAggregateDto
            {
                Query = query,
                ResultsPerMonth = new Dictionary<string, double>()
            };

            var startDate = new DateTime(2018, 1, 1);
            var endDate = DateTime.Today;

            var date = startDate;
            while (date < endDate)
            {
                var toDate = date.AddMonths(1).AddDays(-1);
                var thisMonthResult = GetNumberOfResults(query, date.ToString("MM/dd/yyyy"), toDate.ToString("MM/dd/yyyy"));
                result.ResultsPerMonth.Add(date.ToString("MM-yyyy"), thisMonthResult);

                date=date.AddMonths(1);
            }

            return result;
        }

        private double GetNumberOfResults(string searchPhrases, string fromDate = null,
            string toDate = null)
        {
            _driver.Url =
                $"https://www.google.com/search?q=&tbs=cdr:1,cd_min:{fromDate},cd_max:{toDate}";
            Extensions.DelayRandom(500,1000);

            var query = BuildQuery(searchPhrases);

            var searchBox = _driver.FindWaitElementForClick("//input[contains(@aria-label, 'Search')]");
            searchBox.SendKeys(query);
            searchBox.SendKeys( Keys.Enter);
            Extensions.DelayRandom(500, 1000);

            //click Tools button
            _driver.FindWaitElementForClick("//div[contains(text(), 'Tools')]").Click();

            var howManyResults = _driver.FindElement(By.Id("result-stats"));
            Assert.IsNotNull(howManyResults);

            bool hasResults = _driver.GetWait().Until(ExpectedConditions.TextToBePresentInElement(howManyResults, "results"));
            if (!hasResults)
            {
                return 0;
            }

            var resultText = howManyResults.Text;
            var numOfResults = ExtractResultCount(resultText);
            return numOfResults;

        }

        private double ExtractResultCount(string text)
        {
            var matches = Regex.Matches(text, "\\s*([^\\s]+)\\s+result");
            if (!matches.Any() || matches.First().Groups.Count<2) return 0;
            var numOfResults = double.Parse(matches.First().Groups[1].Value);
            return numOfResults;
        }
    }
}