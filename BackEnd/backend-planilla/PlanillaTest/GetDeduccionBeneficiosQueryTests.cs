using backend_planilla.Application;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanillaTest
{
    public class GetDeduccionBeneficiosQueryTest
    {
        private Mock<IEmpleadoRepository> _mockEmpleadoRepo;
        private Mock<IBeneficiosRepository> _mockBeneficiosRepo;
        private Mock<IBeneficioRepository> _mockBeneficioRepo;
        private GetDeduccionBeneficiosQuery _query;

        [SetUp]
        public void Setup()
        {
            _mockEmpleadoRepo = new Mock<IEmpleadoRepository>();
            _mockBeneficiosRepo = new Mock<IBeneficiosRepository>();
            _mockBeneficioRepo = new Mock<IBeneficioRepository>();

            _query = new GetDeduccionBeneficiosQuery(
                _mockEmpleadoRepo.Object,
                _mockBeneficiosRepo.Object,
                _mockBeneficioRepo.Object
            );
        }

        [Test]
        public async Task DeduccionPorcentajeCorrecta()
        {
            string correo = "usuario@correo.com";
            string cedula = "1-2345-6789";
            decimal salarioBruto = 1000000;

            _mockBeneficioRepo.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns(cedula);
            _mockEmpleadoRepo.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salarioBruto);
            _mockEmpleadoRepo.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(new List<DeduccionBeneficioModel>
            {
                new DeduccionBeneficioModel { IDBeneficio = 1, Nombre = "Ahorro", Tipo = "porcentaje" }
            });
            _mockBeneficiosRepo.Setup(r => r.ObtenerParametrosBeneficio(1)).Returns(new List<ParametroBeneficioModel>
            {
                new ParametroBeneficioModel { TipoValorParametro = "porcentaje", ValorDelParametro = 10 }
            });

            var resultado = await _query.CalcularDeduccioensBeneficios(correo);

            Assert.That(resultado.Count, Is.EqualTo(3)); // relleno hasta 3
            Assert.That(resultado[0].NombreBeneficio, Is.EqualTo("Ahorro"));
            Assert.That(resultado[0].MontoReducido, Is.EqualTo(100000));
        }

        [Test]
        public async Task DeduccionFijaCorrecta()
        {
            string correo = "usuario@correo.com";
            string cedula = "1-2345-6789";
            decimal salarioBruto = 900000;

            _mockBeneficioRepo.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns(cedula);
            _mockEmpleadoRepo.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salarioBruto);
            _mockEmpleadoRepo.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(new List<DeduccionBeneficioModel>
            {
                new DeduccionBeneficioModel { IDBeneficio = 2, Nombre = "Seguro", Tipo = "fijo" }
            });
            _mockBeneficiosRepo.Setup(r => r.ObtenerParametrosBeneficio(2)).Returns(new List<ParametroBeneficioModel>
            {
                new ParametroBeneficioModel { TipoValorParametro = "fijo", ValorDelParametro = 25000 }
            });

            var resultado = await _query.CalcularDeduccioensBeneficios(correo);

            Assert.That(resultado[0].MontoReducido, Is.EqualTo(25000));
        }

        [Test]
        public async Task DeduccionInvalidaPorTipo()
        {
            string correo = "usuario@correo.com";
            string cedula = "1-2345-6789";
            decimal salarioBruto = 800000;

            _mockBeneficioRepo.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns(cedula);
            _mockEmpleadoRepo.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salarioBruto);
            _mockEmpleadoRepo.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(new List<DeduccionBeneficioModel>
            {
                new DeduccionBeneficioModel{ IDBeneficio = 3, Nombre = "Extra", Tipo = "desconocido"}
            });

            var resultado = await _query.CalcularDeduccioensBeneficios(correo);

            Assert.That(resultado[0].MontoReducido, Is.EqualTo(0));
        }
    }
}
