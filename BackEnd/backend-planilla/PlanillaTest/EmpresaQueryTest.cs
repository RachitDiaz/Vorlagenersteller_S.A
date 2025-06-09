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
        public void InformacionValida()
        {
            AgregarEmpresaModel empresa = new()
            {
                CedulaJuridica = "2-125-215122",
                CedulaDueno = "3-7231-1126 ",
                TipoDePago = "Quincenal",
                RazonSocial = "Solucion de problemas",
                Nombre = "Soluciones Hernandez",
                Descripcion = "Soluciones a muchos problemas variados",
                BeneficiosMaximos = "3",
                Correo = "loremipsum@gmail.com",
                Telefono = "1251-1222",
                Provincia = "San Jose",
                Canton = "Montes de Oca",
                Distrito = "San Pedro",
                OtrasSenas = "Sotano de monster pizza"
            };
            string correo = "";
            bool respuesta = true;
            bool resultadoEsperado = true;

            _mockRepo.Setup(repo => repo.RegistrarEmpresa(empresa, correo)).Returns(respuesta);

            var resultado = _empresaQuery.RegistrarEmpresa(empresa, correo);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void InformacionPagoInvalido()
        {
            AgregarEmpresaModel empresa = new()
            {
                CedulaJuridica = "2-125-215122",
                CedulaDueno = "3-7231-1126 ",
                TipoDePago = "Bimensual",
                RazonSocial = "Solucion de problemas",
                Nombre = "Soluciones Hernandez",
                Descripcion = "Soluciones a muchos problemas variados",
                BeneficiosMaximos = "3",
                Correo = "loremipsum@gmail.com",
                Telefono = "1251-1222",
                Provincia = "San Jose",
                Canton = "Montes de Oca",
                Distrito = "San Pedro",
                OtrasSenas = "Sotano de monster pizza"
            };
            string correo = "";
            bool respuesta = true;
            bool resultadoEsperado = false;

            _mockRepo.Setup(repo => repo.RegistrarEmpresa(empresa, correo)).Returns(respuesta);

            var resultado = _empresaQuery.RegistrarEmpresa(empresa, correo);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
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