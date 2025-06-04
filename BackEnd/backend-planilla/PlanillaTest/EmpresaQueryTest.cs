using backend_planilla.Infraestructure;
using backend_planilla.Application;
using backend_planilla.Domain;
using Moq;

namespace PlanillaTest
{
    public class EmpresaQueryTest
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
        public void InformacionVacia()
        {
            AgregarEmpresaModel empresa = new()
            {
                CedulaJuridica = "",
                CedulaDueno = "",
                TipoDePago = "",
                RazonSocial = "",
                Nombre = "",
                Descripcion = "",
                BeneficiosMaximos = "",
                Correo = "",
                Telefono = "",
                Provincia = "",
                Canton = "",
                Distrito = "",
                OtrasSenas = ""
            };
            string correo = "";
            bool respuesta = true;
            bool resultadoEsperado = false;

            _mockRepo.Setup(repo => repo.RegistrarEmpresa(empresa, correo)).Returns(respuesta);

            var resultado = _empresaQuery.RegistrarEmpresa(empresa, correo);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }
    }
}