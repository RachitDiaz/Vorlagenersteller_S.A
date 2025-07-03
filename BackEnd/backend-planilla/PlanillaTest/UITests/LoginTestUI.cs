using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;

namespace PlanillaTest.UITests
{
    [TestFixture]
    public class LoginTestUI

    {
        IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver();
        }

        [Test]
        public void Enter_To_List_Of_Countries_Test()
        {
            var URL = "http://localhost:8080/";

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(URL);

        }
    }
}