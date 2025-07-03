using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using SeleniumExtras.WaitHelpers;

namespace PlanillaTest.GenerarPlanillaTests
{
    public class GenerarCalculosQueryTests
    {
        private Mock<IGenerarCalculosRepository> _repoMock = null!;
        private Mock<IGetDeduccionBeneficiosQuery> _beneficiosMock = null!;
        private Mock<ICalculoDeduccionesObligatorias> _calculadoraMock = null!;
        private GenerarCalculosQuery _query = null!;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IGenerarCalculosRepository>();
            _beneficiosMock = new Mock<IGetDeduccionBeneficiosQuery>();
            _calculadoraMock = new Mock<ICalculoDeduccionesObligatorias>();

            _query = new GenerarCalculosQuery(_repoMock.Object, _beneficiosMock.Object);
        }

        [Test]
        public async Task ObtenerResultadosAsync_Mensual_DeberiaCalcularDeduccionesYBeneficios()
        {
            var cedula = "123";
            var empleados = new List<(string CedulaEmpleado, decimal Salario)>
            {
                ("111", 1000)
            };

            var deducciones = new DeduccionesObligatoriasModel
            {
                SEMEmpleado = 10,
                IVMEmpleado = 20,
                BPPOEmpleado = 5,
                ImpuestoRenta = 15
            };

            var beneficios = new DeduccionCalculadaConTotal
            {
                DeduccionesCalculadas = new List<DeduccionCalculada>
                {
                    new() { NombreBeneficio = "Seguro", MontoReducido = 50 },
                    new() { NombreBeneficio = "Asociación", MontoReducido = 25 },
                    new() { NombreBeneficio = "Otro", MontoReducido = 10 }
                },
                Total = 85
            };

            _repoMock.Setup(r => r.ObtenerEmpleadosConSalarioAsync(cedula)).ReturnsAsync(empleados);
            _calculadoraMock.Setup(c => c.CalcularDeduccionMensual(1000)).Returns(deducciones);
            _beneficiosMock.Setup(b => b.CalcularDeduccionesBeneficios("111")).ReturnsAsync(beneficios);

            var resultado = await _query.ObtenerResultadosAsync(cedula, "mensual", _calculadoraMock.Object, _beneficiosMock.Object);

            Assert.That(resultado.Count, Is.EqualTo(1));
            Assert.That(resultado[0].CedulaEmpleado, Is.EqualTo("111"));
            Assert.That(resultado[0].SalarioBruto, Is.EqualTo(1000));
            Assert.That(resultado[0].Beneficios.Total, Is.EqualTo(85));
        }
    }
}
