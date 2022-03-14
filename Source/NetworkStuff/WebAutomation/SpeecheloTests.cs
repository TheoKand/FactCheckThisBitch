using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebAutomation
{
    public class SpeecheloTests
    {
        private Speechelo _speechelo;

        [SetUp]
        public void Setup()
        {
            _speechelo = new Speechelo();
            _speechelo.Setup();
        }

        [Test]
        public void GenerateAllNarrations()
        {
            int sleepSeconds = 2;

            _speechelo.GenerateNarration("first this is the number one", Path.Combine(_speechelo.SaveLocation,"1.mp3"));
            Thread.Sleep(sleepSeconds * 1000);

            _speechelo.GenerateNarration("second this is the number two", Path.Combine(_speechelo.SaveLocation, "2.mp3"));
            Thread.Sleep(sleepSeconds * 1000);

            _speechelo.GenerateNarration("third this is the number three", Path.Combine(_speechelo.SaveLocation, "3.mp3"));
            Thread.Sleep(sleepSeconds * 1000);

            //var duration = Extensions.GetMp3Duration(mp3LocalPath);
            //TestContext.WriteLine($"File {mp3LocalPath} duration {duration.TotalSeconds}");
        }

        [TearDown]
        public void Teardown()
        {
            _speechelo.Teardown();
        }

    }
}
