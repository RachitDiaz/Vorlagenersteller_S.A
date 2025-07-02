using backend_planilla.Application;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanillaTest.UnitTests
{
    public class GetDeduccionBeneficiosQueryTest
    {
        private Mock<IEmpleadoRepository> _repoEmpleado;
        private Mock<IBeneficiosRepository> _repoBeneficios;
        private Mock<IBeneficioRepository> _repoBeneficio;
        private GetDeduccionBeneficiosQuery _query;

        [SetUp]
        public void Setup()
        {
            _repoEmpleado = new Mock<IEmpleadoRepository>();
            _repoBeneficios = new Mock<IBeneficiosRepository>();
            _repoBeneficio = new Mock<IBeneficioRepository>();

            _query = new GetDeduccionBeneficiosQuery(
                _repoEmpleado.Object,
                _repoBeneficios.Object,
                _repoBeneficio.Object
            );
        }

        [Test]
        public async Task DeduccionPorcentajeCorrecta()
        {
            string cedula = "1-2345-6789";
            string correo = "empleado@test.com";
            decimal salario = 1000;

            _repoEmpleado.Setup(r => r.ObtenerCorreoDesdeCedula(cedula)).ReturnsAsync(correo);
            _repoBeneficio.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns(cedula);
            _repoEmpleado.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salario);

            var beneficios = new List<DeduccionBeneficioModel>
            {
                new DeduccionBeneficioModel { IDBeneficio = 1, Nombre = "Seguro", Tipo = "porcentaje" }
            };
            _repoEmpleado.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(beneficios);

            var parametros = new List<ParametroBeneficioModel>
            {
                new ParametroBeneficioModel { TipoValorParametro = "porcentaje", ValorDelParametro = 5 }
            };
            _repoBeneficios.Setup(r => r.ObtenerParametrosBeneficio(1)).Returns(parametros);

            var resultado = await _query.CalcularDeduccionesBeneficios(cedula);

            Assert.AreEqual(3, resultado.DeduccionesCalculadas.Count()); 
            Assert.AreEqual("Seguro", resultado.DeduccionesCalculadas[0].NombreBeneficio);
            Assert.AreEqual(50m, resultado.DeduccionesCalculadas[0].MontoReducido);
        }

        [Test]
        public async Task DeduccionMontoFijoCorrecta()
        {
            string cedula = "2-2222-2222";
            string correo = "fijo@test.com";
            decimal salario = 800;

            _repoEmpleado.Setup(r => r.ObtenerCorreoDesdeCedula(cedula)).ReturnsAsync(correo);
            _repoBeneficio.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns(cedula);
            _repoEmpleado.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salario);

            var beneficios = new List<DeduccionBeneficioModel>
            {
                new DeduccionBeneficioModel { IDBeneficio = 2, Nombre = "Café", Tipo = "fijo" }
            };
            _repoEmpleado.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(beneficios);

            var parametros = new List<ParametroBeneficioModel>
            {
                new ParametroBeneficioModel { TipoValorParametro = "fijo", ValorDelParametro = 200 }
            };
            _repoBeneficios.Setup(r => r.ObtenerParametrosBeneficio(2)).Returns(parametros);

            var resultado = await _query.CalcularDeduccionesBeneficios(cedula);

            Assert.AreEqual("Café", resultado.DeduccionesCalculadas[0].NombreBeneficio);
            Assert.AreEqual(200, resultado.DeduccionesCalculadas[0].MontoReducido);
        }

        [Test]
        public async Task CompletaHastaTresBeneficios()
        {
            string cedula = "3-3333-3333";
            string correo = "completo@test.com";
            decimal salario = 1200;

            _repoEmpleado.Setup(r => r.ObtenerCorreoDesdeCedula(cedula)).ReturnsAsync(correo);
            _repoBeneficio.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns(cedula);
            _repoEmpleado.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salario);

            var beneficios = new List<DeduccionBeneficioModel>();
            _repoEmpleado.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(beneficios);

            var resultado = await _query.CalcularDeduccionesBeneficios(cedula);

            Assert.AreEqual(3, resultado.DeduccionesCalculadas.Count);
            Assert.IsTrue(resultado.DeduccionesCalculadas.TrueForAll(d => d.NombreBeneficio == "Sin beneficio"));
        }
    }
}
