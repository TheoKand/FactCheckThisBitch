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

        private string _sites =
            "bbc.co.uk,bbc.com,dailymail.co.uk,theguardian.com,express.co.uk,thesun.co.uk,mirror.co.uk,independent.co.uk,news.sky.com,msn.com,telegraph.co.uk";

        private string GetQuery(string query)
        {
            var siteList = string.Join(" OR ", _sites.Split(',').Select(_ => $"site:{_}"));
            var result = $"\"{query}\" and ({siteList})";
            return result;
        }

        [SetUp]
        public void StartBrowser()
        {
            if (_driver != null) return;

            _driver = new ChromeDriver(_chromedriverLocation);
            _driver.Manage().Window.Maximize();
            Extensions.DelayRandom();
            _driver.Url = $"https://www.google.com/search?q=vaccination center near me";
            
            

            //click agree button
            try
            {
                _driver.FindWaitElementForClick("//*[contains(text(), 'I agree')]", 10)?.Click();
            }
            catch { }
           
        }

        [Theory]
        [TestCase("died suddenly")]
        [TestCase("sudden death")]
        [TestCase("unexpected death")]
        [TestCase("cardiac arrest")]
        [TestCase("sudden heart attack")]
        [TestCase("dies of heart attack")]
        public void TestQueries(string query,bool onlyNews = true)
        {
            var result = GetGoogleResultsAggregate(query, true);
            TestContext.Out.WriteLine($"{result}");
        }

        [TestCase("arrive at")]
        [TestCase("one week")]
        [TestCase("tax return")]
        [TestCase("two months")]
        [TestCase("take decades")]
        [TestCase("tectonic shift")]
        public void ControlQueries(string query, bool onlyNews = true)
        {
            var result = GetGoogleResultsAggregate(query, true);
            TestContext.Out.WriteLine($"{result}");
        }

        private GoogleResultsAggregateDto GetGoogleResultsAggregate(string query, bool onlyNews = false)
        {
            var result = new GoogleResultsAggregateDto
            {
                Query = query, OnlyNews = onlyNews, ResultsPerYear = new Dictionary<int, double>()
            };

            for (var year = 2016; year < 2023; year++)
            {
                var fromDate = $"1/1/{year}";
                var toDate = $"12/31/{year}";
                var thisYearResult = NumberOfGoogleResultsForQuery(query, onlyNews, fromDate, toDate);
                result.ResultsPerYear.Add(year,thisYearResult);
            }

            return result;
        }

        private double NumberOfGoogleResultsForQuery(string query, bool onlyNews = false, string fromDate = null,
            string toDate = null)
        {
            _driver.Url =
                $"https://www.google.com/search?q={GetQuery(query)}&tbs=cdr:1,cd_min:{fromDate},cd_max:{toDate}";

            
            //click Tools button
            _driver.FindWaitElementForClick("//div[contains(text(), 'Tools')]").Click();

            var howManyResults = _driver.FindElement(By.Id("result-stats"));
            Assert.IsNotNull(howManyResults);

            bool hasResults = _driver.GetWait().Until(ExpectedConditions.TextToBePresentInElement(howManyResults, "results"));
            Assert.True(hasResults);

            var numOfResults = double.Parse(Regex.Matches(howManyResults.Text, "About\\s+(.+)\\s+results").First()?.Groups[1].Value);

            //TestContext.Out.WriteLine($"{query} {fromDate}-{toDate}: {numOfResults}");

            return numOfResults;

        }

        [TearDown]
        public void CloseBrowser()
        {
            //_driver.Close();
        }
    }
}