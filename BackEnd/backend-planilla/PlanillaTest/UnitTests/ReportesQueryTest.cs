using backend_planilla.Infraestructure;
using backend_planilla.Application;
using backend_planilla.Domain;
using Moq;
using backend_planilla.Models;

namespace PlanillaTest.UnitTests
{
    public class ReportesQueryTest
    {
        private Mock<IReportesRepository> _mockRepo;
        private IReportesQuery _ReportesQuery;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IReportesRepository>();
            _ReportesQuery = new ReportesQuery(_mockRepo.Object);
        }

        [Test]
        public void ObtenerUltimosPagosEmpleadoCasoIdeal()
        {
            string cedula = "1-2222-3333";
            int cantidad = 10;
            List<ReportePagoEmpleadoDTO> respuesta = new List<ReportePagoEmpleadoDTO>();

            _mockRepo.Setup(repo => repo.ObtenerUltimosPagosEmpleado(cedula, cantidad)).Returns(respuesta);

            List<ReportePagoEmpleadoDTO>? resultadoEsperado = respuesta;
            List<ReportePagoEmpleadoDTO>? resultado = _ReportesQuery.ObtenerUltimosPagosEmpleado(cedula);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void ObtenerUltimosPagosEmpleadoFormatoErroneo()
        {
            string cedula = "1_2222_3333";
            int cantidad = 10;
            List<ReportePagoEmpleadoDTO> respuesta = new List<ReportePagoEmpleadoDTO>();

            _mockRepo.Setup(repo => repo.ObtenerUltimosPagosEmpleado(cedula, cantidad)).Returns(respuesta);

            List<ReportePagoEmpleadoDTO>? resultadoEsperado = null;
            List<ReportePagoEmpleadoDTO>? resultado = _ReportesQuery.ObtenerUltimosPagosEmpleado(cedula);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void ObtenerUltimosPagosEmpresaCasoIdeal()
        {
            string cedula = "1-2222-3333";
            int cantidad = 10;
            List<ReportePagoEmpresaDTO> respuesta = new List<ReportePagoEmpresaDTO>();

            _mockRepo.Setup(repo => repo.ObtenerUltimosPagosEmpresa(cedula, cantidad)).Returns(respuesta);

            List<ReportePagoEmpresaDTO>? resultadoEsperado = respuesta;
            List<ReportePagoEmpresaDTO>? resultado = _ReportesQuery.ObtenerUltimosPagosEmpresa(cedula);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void ObtenerUltimosPagosEmpresaFormatoErroneo()
        {
            string cedula = "1_2222_3333";
            int cantidad = 10;
            List<ReportePagoEmpresaDTO> respuesta = new List<ReportePagoEmpresaDTO>();

            _mockRepo.Setup(repo => repo.ObtenerUltimosPagosEmpresa(cedula, cantidad)).Returns(respuesta);

            List<ReportePagoEmpresaDTO>? resultadoEsperado = null;
            List<ReportePagoEmpresaDTO>? resultado = _ReportesQuery.ObtenerUltimosPagosEmpresa(cedula);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }
    }
}