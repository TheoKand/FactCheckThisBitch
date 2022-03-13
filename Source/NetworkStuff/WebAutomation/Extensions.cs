using NAudio.Wave;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Threading;


namespace WebAutomation
{
    public static class Extensions
    {
        public static double SecondsTimeout = 60;

        public static IWebElement FindWaitElement(this IWebDriver driver,string xPath, double? timeout=null)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout ?? SecondsTimeout));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IWebElement element = wait.Until( ExpectedConditions.ElementExists(By.XPath(xPath)));
            FackCheckThisBitch.Common.Extensions.DelayRandom();
            return element;
        }

        public static IWebElement FindWaitElement(this IWebDriver driver, By by, double? timeout = null)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout ?? SecondsTimeout));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IWebElement element = wait.Until(ExpectedConditions.ElementExists(by));
            return element;
        }

        public static IWebElement FindWaitElementForClick(this IWebDriver driver, string xPath, double? timeout = null)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout??SecondsTimeout));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException), typeof(ElementNotInteractableException));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPath)));
            FackCheckThisBitch.Common.Extensions.DelayRandom(500,1000);
            return element;
        }

        public static WebDriverWait GetWait(this IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(SecondsTimeout));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait;
        }


        public static string GetDom(this IWebDriver driver)
        {
            IWebElement dom = driver.FindElement(By.XPath("//*"));
            var innerHtml = dom.GetAttribute("innerHTML");
            return innerHtml;
        }

        public static void SaveFile(string url, string filePath)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            byte[] buffer = client.DownloadData(url);
            Stream stream = new FileStream(filePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(buffer);
            stream.Close();
        }

        public static TimeSpan GetMp3Duration(string filePath)
        {
            Mp3FileReader reader = new Mp3FileReader(filePath);
            TimeSpan duration = reader.TotalTime;
            return duration;

        }

    }
}
