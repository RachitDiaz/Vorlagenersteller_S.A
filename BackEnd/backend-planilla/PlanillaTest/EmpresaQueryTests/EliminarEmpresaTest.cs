using backend_planilla.Infraestructure;
using backend_planilla.Application;
using backend_planilla.Domain;
using Moq;

namespace PlanillaTest.EmpresaQueryTests
{
    public class EliminarEmpresaTest
    {
        private Mock<IEmpresaRepository> _mockRepo;
        private IEmpresaQuery _empresaQuery;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IEmpresaRepository>();
            _empresaQuery = new EmpresaQuery(_mockRepo.Object);
        }

        [Test]
        public void EliminarEmpresa_Throws_InvalidOperationException_On_NonExistingEmpresa()
        {
            string correoSinEmpresa = "NoHayEmpresa";
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correoSinEmpresa)).Throws(new InvalidOperationException());
            Assert.Throws<InvalidOperationException>(() => _empresaQuery.EliminarEmpresa(correoSinEmpresa));
        }

        [Test]
        public void EliminarEmpresa_Throws_Exception_On_UnexpectedError_In_Query()
        {
            string correoSinEmpresa = "NoHayEmpleado";
            _mockRepo.Setup(r => r.EliminarEmpresa(correoSinEmpresa)).Throws(new Exception());
            Assert.Throws<Exception>(() => _empresaQuery.EliminarEmpresa(correoSinEmpresa));
        }

        [Test]
        public void EliminarEmpresa_Returns_List_On_ValidEmail()
        {
            string correoConEmpresa = "TengoEmpresa@example.com";
            string cedulaEmpresa = "CedulaConEmpresa";
            var correosEsperados = new List<string> { "email1@example.com", "email2@example.com" };
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correoConEmpresa)).Returns(cedulaEmpresa);
            _mockRepo.Setup(r => r.EliminarEmpresa(cedulaEmpresa)).Returns(correosEsperados);

            var resultadoEliminar = _empresaQuery.EliminarEmpresa(correoConEmpresa);

            Assert.IsTrue(resultadoEliminar);
        }
    }
}