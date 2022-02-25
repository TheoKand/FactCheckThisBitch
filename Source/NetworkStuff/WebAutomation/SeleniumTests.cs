using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FackCheckThisBitch.Common;
using SeleniumExtras.WaitHelpers;

namespace WebAutomation
{


    public class SeleniumTests
    {
        private IWebDriver _driver;

        private string _chromedriverLocation =
            "C:\\Users\\theok\\Repos\\FactCheckThisBitch\\Source\\NetworkStuff\\WebAutomation";

        private string _initialQuery = "vaccination center near me";
        private string _testPhrases = "died suddenly,dies of heart attack,his unexpected death,his sudden death,her sudden death,her unexpected death,died of cardiac arrest";
        private string _controlPhrases = "best of both worlds";
        private string _sites = "bbc.co.uk,dailymail.co.uk,theguardian.com,express.co.uk,thesun.co.uk,msn.com,foxnews.com,abcnews.go.com,nbcnews.com,cnn.com,news.yahoo.com,cbsnews.com,usnews.com";

        private string BuildQuery(string query)
        {
            var phrasesList = string.Join(" OR ", query.Split(',').Select(_ => $"\"{_}\""));

            var siteList = string.Join(" OR ", _sites.Split(',').Select(_ => $"site:{_}"));
            var result = $"({phrasesList}) and ({siteList}) -COVID";
            return result;
        }

        public SeleniumTests()
        {
            _driver = new ChromeDriver(_chromedriverLocation);
            _driver.Url = "https://www.google.com";
            _driver.Manage().Window.Maximize();
            Extensions.DelayRandom();
            _driver.Url = $"https://www.google.com/search?q={_initialQuery}";
            
            //click agree button
            try
            {
                _driver.FindWaitElementForClick("//*[contains(text(), 'I agree')]", 10)?.Click();
            }
            catch { }
           
        }

        [Test]
        public void TestQueriesYearlyTotals()
        {
            var result = GetGoogleResultsYearlyAggregate(_testPhrases);
            TestContext.Out.WriteLine($"{result}");
        }

        [Test]
        public void ControlQueriesYearlyTotals()
        {
            var result = GetGoogleResultsYearlyAggregate(_controlPhrases);
            TestContext.Out.WriteLine($"{result}");
        }

        [Test]
        public void TestQueriesPerMonth()
        {
            var result = GetGoogleResultsMonthlyAggregate(_testPhrases);
            TestContext.Out.WriteLine($"{result}");
        }

        [Theory]
        [TestCase("at first glance")]
        [TestCase("critics say")]
        [TestCase("begs the question")]
        [TestCase("needless to say")]
        [TestCase("make no mistake")]

        public void ControlQueriesPerMonth(string controlPhrase)
        {
            var result = GetGoogleResultsMonthlyAggregate(controlPhrase);
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
                var thisMonthResult = NumberOfGoogleResultsForQuery(query, date.ToString("MM/dd/yyyy"), toDate.ToString("MM/dd/yyyy"));
                result.ResultsPerMonth.Add(date.ToString("MM-yyyy"), thisMonthResult);

                date=date.AddMonths(1);
            }

            return result;
        }

        private GoogleResultsAggregateDto GetGoogleResultsYearlyAggregate(string query)
        {
            var result = new GoogleResultsAggregateDto
            {
                Query = query, ResultsPerYear = new Dictionary<int, double>()
            };

            for (var year = 2018; year < 2023; year++)
            {
                var fromDate = $"1/1/{year}";
                var toDate = $"12/31/{year}";
                var thisYearResult = NumberOfGoogleResultsForQuery(query,  fromDate, toDate);
                result.ResultsPerYear.Add(year,thisYearResult);
            }

            return result;
        }

        private double NumberOfGoogleResultsForQuery(string searchPhrases, string fromDate = null,
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