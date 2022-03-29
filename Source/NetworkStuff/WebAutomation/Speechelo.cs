using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace WebAutomation
{
    public class Speechelo : IDisposable
    {
        private IWebDriver _driver;

        private readonly string _login = "tkandiliotis@gmail.com";
        private readonly string _password = "1394Hgk8Tplka";
        private readonly string _chromedriverLocation =
            "C:\\Users\\theok\\Repos\\FactCheckThisBitch\\Source\\NetworkStuff\\WebAutomation";

        public string SaveLocation { get; set; }

        public Speechelo()
        {
            SaveLocation = $"C:\\Users\\theok\\Desktop\\test";
        }

        public void Setup()
        {
            _driver = new ChromeDriver(_chromedriverLocation);
            _driver.Url = "https://app.blasteronline.com/speechelo/";
            _driver.Manage().Window.Maximize();

            //type login and password
            _driver.FindWaitElement(By.Id("loginemail"), 5).SendKeys(_login);
            _driver.FindWaitElement(By.Id("loginpassword"), 5).SendKeys(_password);
            _driver.FindWaitElement(By.Id("captcha"), 5).Click();
        }

        public void MiminizeWindow()
        {
            _driver.Manage().Window.Minimize();
        }

        private string IdOfLastGeneratedVoice()
        {
            try
            {
                var lastGeneratedVoiceIdCell =
                    _driver.FindWaitElement("//*[@id='blastered_datatable']/tbody/tr[1]/td[2]", 5);
                return lastGeneratedVoiceIdCell.Text;
            }
            catch
            {
                return null;
            }

        }

        public void GenerateNarration(string narration, string voice = "ttsVoiceen-US-AriaNeural", string audioFilePath = null)
        {
            var txtNarration = _driver.FindWaitElement(By.Id("tts-tarea"), 60);
            txtNarration.Clear();
            txtNarration.SendKeys(narration);

            var radioGrace = _driver.FindWaitElement(By.Id(voice), 20);
            radioGrace.Click();

            var idOfLastVoiceBeforeGeneration = IdOfLastGeneratedVoice();

            _driver.FindWaitElement(By.Id("ttsGenerateBtn"), 10).Click();


            try
            {
                _driver.FindWaitElementForClick(
                        "//button[contains(text(), 'Just generate')]", 5)
                    .Click();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {

            }



            //Application.DoEvents();
            //Thread.Sleep(2 * 1000);
            //Application.DoEvents();


            //wait until voice generated
            while (IdOfLastGeneratedVoice() == idOfLastVoiceBeforeGeneration)
            {
                Thread.Sleep(1000);
                Application.DoEvents();
            }

            _driver.Navigate().Refresh();

            //download mp3 from latest generated voice
            var mp3Anchor = _driver.FindWaitElement("//*[@id='blastered_datatable']/tbody/tr[1]/td[7]/a", 30);
            var mp3Link = mp3Anchor.GetAttribute("href");

            var mp3LocalPath = audioFilePath ?? Path.Combine(SaveLocation, $"{Guid.NewGuid()}.mp3");
            Extensions.SaveFile(mp3Link, mp3LocalPath);

            //close OK dialog
            try
            {
                _driver.FindWaitElementForClick(
                        "/html/body/div[8]/div/div[3]/button[1]", 10)
                    .Click();
            }
            catch
            {
            }

        }

        public void Teardown()
        {
            if (_driver == null) return;

            try
            {
                _driver.Close();
                _driver.Dispose();
            }
            catch
            {
            }

            _driver = null;
        }

        public void Dispose()
        {
            Teardown();
        }
    }
}

