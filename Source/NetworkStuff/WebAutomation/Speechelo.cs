using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework.Internal;

namespace WebAutomation
{
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

    namespace WebAutomation
    {
        public class Speechelo
        {
            private IWebDriver _driver;

            private string _login = "tkandiliotis@gmail.com";
            private string _password = "1394Hgk8Tplka";


            private string _chromedriverLocation =
                "C:\\Users\\theok\\Repos\\FactCheckThisBitch\\Source\\NetworkStuff\\WebAutomation";

            public Speechelo()
            {
                _driver = new ChromeDriver(_chromedriverLocation);
                _driver.Url = "https://app.blasteronline.com/speechelo/";
                _driver.Manage()
                    .Window.Maximize();


                //type login and password
                _driver.FindWaitElement(By.Id("loginemail"), 5).SendKeys(_login);
                _driver.FindWaitElement(By.Id("loginpassword"), 5).SendKeys(_password);
                _driver.FindWaitElement(By.Id("captcha"), 5).Click();

                

            }

            [Test]
            public void GenerateAllNarrations()
            {
                GenerateNarration("Speechelo is wonderful");
                GenerateNarration("Inside Rihanna’s TEN year battle with neighbour who sued her for parking on his drive & planting ‘too high’ bamboo bush");
                GenerateNarration("parking on his drive & planting ‘too high’ bamboo bush");
            }


            public void GenerateNarration(string narration)
            {
                var txtNarration = _driver.FindWaitElement(By.Id("tts-tarea"), 60);
                txtNarration.SendKeys(narration);

                var radioGrace = _driver.FindWaitElement(By.Id("ttsVoiceen-US-AriaNeural"), 5);
                radioGrace.Click();

                _driver.FindWaitElement(By.Id("ttsGenerateBtn"), 5).Click();

                txtNarration.Clear();

                try
                {
                    _driver.FindWaitElementForClick(
                            "//button[contains(text(), 'Just generate the voiceover as it is')]", 5)
                        .Click();
                }
                catch
                {
                }

                var id = Guid.NewGuid().ToString();

                var mp3Anchor = _driver.FindWaitElement("//*[@id='blastered_datatable']/tbody/tr[1]/td[7]/a", 30);
                var mp3Link = mp3Anchor.GetAttribute("href");
                var mp3LocalPath = $"C:\\Users\\theok\\Desktop\\test\\{id}.mp3";
                Extensions.SaveFile(mp3Link, mp3LocalPath);
                var duration = Extensions.GetMp3Duration(mp3LocalPath);
                TestContext.WriteLine($"File {mp3LocalPath} duration {duration.TotalSeconds}");


                //close OK dialog
                _driver.FindWaitElementForClick(
                        "/html/body/div[8]/div/div[3]/button[1]", 10)
                    .Click();

                Thread.Sleep(5*1000);


            }

        }
    }
}
