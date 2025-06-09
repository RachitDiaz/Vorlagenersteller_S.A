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
        private Mock<IEmpleadoRepository> _mockRepo;
        private GetDeduccionBeneficiosQuery _query;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IEmpleadoRepository>();
            _query = new GetDeduccionBeneficiosQuery(_mockRepo.Object);
        }

        [Test]
        public async Task DeduccionPorcentajeCorrecta()
        {
            string cedula = "1-2345-6789";
            decimal salarioBruto = 1000000;

            _mockRepo.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salarioBruto);
            _mockRepo.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(new List<DeduccionBeneficioModel>
            {
                new  DeduccionBeneficioModel { IDBeneficio = 1, Nombre = "Ahorro", Tipo = "porcentaje", Monto = 10, Descripción = "Test1" }
            });

            var resultado = await _query.ExecuteAsync(cedula);

            Assert.That(resultado.Count, Is.EqualTo(1));
            Assert.That(resultado[0].NombreBeneficio, Is.EqualTo("Ahorro"));
            Assert.That(resultado[0].MontoReducido, Is.EqualTo(100000)); 
        }

        [Test]
         public async Task DeduccionFijaCorrecta()
        {
            string cedula = "1-2345-6789";
            decimal salarioBruto = 900000;

            _mockRepo.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salarioBruto);
            _mockRepo.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(new List<DeduccionBeneficioModel>
            {
                new  DeduccionBeneficioModel { IDBeneficio = 2, Nombre = "Seguro", Tipo = "fijo", Monto = 25000, Descripción = "Test2" }
            });

            var resultado = await _query.ExecuteAsync(cedula);

            Assert.That(resultado[0].MontoReducido, Is.EqualTo(25000));
        }

        [Test]
        public async Task DeduccionInvalidaPorTipo()
        {
            string cedula = "1-2345-6789";
            decimal salarioBruto = 800000;

            _mockRepo.Setup(r => r.ObtenerSalarioBruto(cedula)).ReturnsAsync(salarioBruto);
            _mockRepo.Setup(r => r.ObtenerBeneficiosEmpleado(cedula)).ReturnsAsync(new List<DeduccionBeneficioModel>
            {
                new  DeduccionBeneficioModel{ IDBeneficio = 3, Nombre = "Extra", Tipo = "desconocido", Monto = 50000, Descripción = "Test3" }
            });

            var resultado = await _query.ExecuteAsync(cedula);

            Assert.That(resultado[0].MontoReducido, Is.EqualTo(0));
        }
    }
}
