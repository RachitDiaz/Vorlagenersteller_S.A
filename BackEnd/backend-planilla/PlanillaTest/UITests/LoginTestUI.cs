using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace PlanillaTest.UITests
{
    [TestFixture]
    public class LoginTestUI
    {
        IWebDriver _driver;
        WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Enter_To_List_Of_Countries_Test()
        {
            var url = "http://localhost:8080/";
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(url);

            _wait.Until(d => d.FindElement(By.LinkText("Comenzar ahora"))).Click();

            _wait.Until(d => d.FindElement(By.CssSelector(".tab:nth-child(2)"))).Click();

            _wait.Until(d => d.FindElement(By.CssSelector(".form-group:nth-child(1) > input"))).Click();
            _driver.FindElement(By.CssSelector(".form-group:nth-child(1) > input")).SendKeys("shihtangdaniel@gmail.com");

            _driver.FindElement(By.CssSelector(".form-group:nth-child(2) > input")).Click();
            _driver.FindElement(By.CssSelector(".form-group:nth-child(2) > input")).SendKeys("ContraTemporal");

            _driver.FindElement(By.CssSelector(".login-button")).Click();

            var h2 = _wait.Until(d => d.FindElement(By.CssSelector("h2")));
            Assert.That(h2.Text, Is.EqualTo("Gestión de Empleados"));

           // _driver.FindElement(By.CssSelector(".auth-button")).Click();
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}