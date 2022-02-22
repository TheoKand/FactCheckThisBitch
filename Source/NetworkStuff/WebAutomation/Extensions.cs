using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace WebAutomation
{
    public static class Extensions
    {
        public static double SecondsTimeout = 30;

        public static IWebElement FindWaitElement(this IWebDriver driver,string xPath, double? timeout=null)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout ?? SecondsTimeout));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IWebElement element = wait.Until( ExpectedConditions.ElementExists(By.XPath(xPath)));
            return element;
        }

        public static IWebElement FindWaitElementForClick(this IWebDriver driver, string xPath, double? timeout = null)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout??SecondsTimeout));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException), typeof(ElementNotInteractableException));
            IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPath)));
            DelayForClick();
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

        public static void DelayRandom()
        {
            int milliSeconds = new Random().Next(500, 2000);
            Thread.Sleep(milliSeconds);
        }

        public static void MaybeDelay()
        {
            int oneToTen = new Random().Next(1, 10);
            if (oneToTen > 7)
            {
                DelayRandom();
            }

        }

        public static void DelayForClick()
        {
            Thread.Sleep(750);
        }

        public static string GetSign(this double input)
        {
            if (input > 0)
            {
                return "+";
            }

            return "";
        }
    }
}
