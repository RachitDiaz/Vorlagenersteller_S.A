using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UITests
{
    public class UITest004
    {
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        IWebDriver _driver;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
        }

        [Test]
        public void Agregar_Nuevo_Empleado_Y_Eliminar()
        {
            var URL = "http://localhost:8080/";
            var correoDueno = "shihtangdaniel@gmail.com";
            var contrasenaDueno = "ContraTemporal";
            var nombreEmpleado = "Daniel";
            var primerApellido = "Shih";
            var segundoApellido = "Tang";
            var genero = "Masculino";
            var correoEmpleado = "danielshih0@gmail.com";
            var contrasena = "Contrasena";
            var cedulaEmpleado = "1-0938-4839";
            var cuentaIBAN = "CR12039192874";
            var salarioBruto = "10000000";
            var tipoContrato = "Tiempo completo";
            _driver.Manage().Window.Maximize();

            _driver.Navigate().GoToUrl(URL);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            var comenzarAhora = wait.Until(d => d.FindElement(By.XPath("//a[text()='Comenzar ahora']")));
            comenzarAhora.Click();

            var dueno = wait.Until(d => d.FindElement(By.CssSelector("button.tab")));
            var botones = _driver.FindElements(By.CssSelector("button.tab"));
            foreach (var boton in botones)
            {
                if (boton.Text.Trim() == "Dueño")
                {
                    boton.Click();
                    break;
                }
            }

            var correoInput = wait.Until(d => d.FindElement(By.CssSelector("input[type='email']")));
            correoInput.SendKeys(correoDueno);

            var contrasenaInput = _driver.FindElement(By.CssSelector("input[type='password']"));
            contrasenaInput.SendKeys(contrasenaDueno);
            _driver.FindElement(By.CssSelector("button.login-button")).Click();

            wait.Until(d => d.FindElements(By.CssSelector("table tbody tr")).Count > 0);

            var agregarEmpleado = _driver.FindElement(By.CssSelector("button.add-button"));
            agregarEmpleado.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#modalAgregarEmpleado.show")));

            _driver.FindElement(By.CssSelector("#modalAgregarEmpleado input[type='text'][class*='form-control']")).SendKeys(nombreEmpleado);
            var inputs = _driver.FindElements(By.CssSelector("#modalAgregarEmpleado input[type='text'][class*='form-control']"));
            inputs[1].SendKeys(primerApellido);
            inputs[2].SendKeys(segundoApellido);

            var modal = _driver.FindElement(By.Id("modalAgregarEmpleado"));
            var selectElements = modal.FindElements(By.CssSelector("select.form-select"));
            var selectGenero = new SelectElement(selectElements[0]);
            selectGenero.SelectByText(genero);

            _driver.FindElement(By.CssSelector("#modalAgregarEmpleado input[type='email']")).SendKeys(correoEmpleado);
            _driver.FindElement(By.CssSelector("#modalAgregarEmpleado input[type='password']")).SendKeys(contrasena);
            _driver.FindElement(By.CssSelector("#modalAgregarEmpleado input[placeholder='x-xxxx-xxxx']")).SendKeys(cedulaEmpleado);
            inputs[4].SendKeys(cuentaIBAN);
            _driver.FindElement(By.CssSelector("#modalAgregarEmpleado input[type='number']")).SendKeys(salarioBruto);


            var selectContrato = new SelectElement(selectElements[1]);
            selectContrato.SelectByText(tipoContrato);
            _driver.FindElement(By.CssSelector("#modalAgregarEmpleado button[type='submit']")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            IAlert alert = _driver.SwitchTo().Alert();
            string alertText = alert.Text;
            if (alertText == "Empleado registrado con éxito")
            {
                alert.Accept();
            }
            else
            {
                throw new Exception(alertText);
            }

            _driver.Navigate().Refresh();
            wait.Until(drv => drv.FindElement(By.CssSelector("table tbody tr")));

            var filas = _driver.FindElements(By.CssSelector("table tbody tr"));

            bool existe = false;
            foreach (var fila in filas)
            {
                if (fila.Text.Contains(nombreEmpleado) && fila.Text.Contains(cedulaEmpleado))
                {
                    existe = true;
                    break;
                }
            }
            if (!existe)
            {
                throw new Exception("El nuevo empleado no se encontró en la lista.");
            }

            filas = _driver.FindElements(By.CssSelector("table tbody tr"));

            IWebElement targetRow = null;
            foreach (var fila in filas)
            {
                if (fila.Text.Contains(cedulaEmpleado))
                {
                    targetRow = fila;
                    break;
                }
            }

            if (targetRow == null)
            {
                throw new Exception("Empleado no encontrado para eliminar.");
            }

            var eliminarEmpleado = targetRow.FindElement(By.CssSelector("button.delete"));
            eliminarEmpleado.Click();

            var confirmarEliminar = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("#myModal button.btn-danger")));
            confirmarEliminar.Click();

            wait.Until(drv => drv.Url.EndsWith("/ListaEmpleados"));

            wait.Until(drv =>
            {
                var updatedRows = drv.FindElements(By.CssSelector("table tbody tr"));
                return !updatedRows.Any(r => r.Text.Contains(cedulaEmpleado));
            });
        }
    }
}