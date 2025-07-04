using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace UITests
{
    public class SeleniumTests
    {
        IWebDriver _driver;
        WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Añadir_Beneficio_MediSeguro_Con_Sus_Dependientes()
        {
            var URL = "http://localhost:8080/";
            _driver.Navigate().GoToUrl(URL);
            _driver.Manage().Window.Maximize();

            var loginButton = _wait.Until(driver =>
                driver.FindElement(By.XPath("//button[contains(text(),'Iniciar sesión')]")));
            loginButton.Click();

            var correo = _wait.Until(driver =>
                driver.FindElement(By.XPath("//input[@placeholder='Correo de la empresa']")));
            correo.SendKeys("pedro.martinez@gmail.com");

            var pass = _driver.FindElement(By.XPath("//input[@type='password']"));
            pass.SendKeys("Contrasena");

            var ingresarBtn = _driver.FindElement(By.XPath("//button[contains(text(),'Ingresar')]"));
            ingresarBtn.Click();

            var beneficiosEmpleado = _wait.Until(driver =>
                driver.FindElement(By.LinkText("Beneficios Empleado")));
            beneficiosEmpleado.Click();

            _wait.Until(driver =>
                driver.FindElements(By.ClassName("benefit-item")).Count > 0);

            var items = _driver.FindElements(By.ClassName("benefit-item"));
            IWebElement botonAgregarMediSeguro = null;

            foreach (var item in items)
            {
                if (item.Text.Contains("MediSeguro", StringComparison.OrdinalIgnoreCase))
                {
                    botonAgregarMediSeguro = item.FindElement(By.XPath(".//button[contains(text(),'Agregar')]"));
                    break;
                }
            }

            Assert.IsNotNull(botonAgregarMediSeguro, "No se encontró el botón para MediSeguro");
            botonAgregarMediSeguro.Click();

            var input = _wait.Until(driver =>
                driver.FindElement(By.Id("dependientesInput")));
            input.Clear();
            input.SendKeys("3");

            var aceptarModal = _driver.FindElement(By.XPath("//div[contains(@class,'modal-actions')]/button[contains(text(),'Aceptar')]"));
            aceptarModal.Click();

            _wait.Until(driver =>
            {
                try
                {
                    var alert = _driver.SwitchTo().Alert();
                    return alert != null;
                }
                catch (NoAlertPresentException)
                {
                    return false;
                }
            });

            var alerta = _driver.SwitchTo().Alert();
            string textoAlerta = alerta.Text;
            Assert.IsTrue(textoAlerta.Contains("actualizados correctamente"), "Texto inesperado en la alerta.");
            alerta.Accept();

            Assert.Pass("Beneficio MediSeguro añadido exitosamente con 3 dependientes.");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
