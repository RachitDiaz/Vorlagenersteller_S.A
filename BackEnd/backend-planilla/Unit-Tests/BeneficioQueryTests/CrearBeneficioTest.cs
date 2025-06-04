using Moq;
using backend_planilla.Application;
using backend_planilla.Infraestructure;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using backend_planilla.Exceptions;

namespace Unit_Tests.BeneficioQueryTests
{
    public class CrearBeneficioTest
    {
        private BeneficiosQuery _beneficiosHandler;
        private Mock<IBeneficiosRepository> _mockRepo;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IBeneficiosRepository>();
            _beneficiosHandler = new BeneficiosQuery(_mockRepo.Object);
        }

        [Test]
        public void CrearBeneficio_Throws_FormatException_On_EmptyEmail()
        {
            BeneficioModel beneficio = new();
            Assert.Throws<FormatException>(() => _beneficiosHandler.CrearBeneficio(beneficio, ""));
        }

        [Test]
        public void CrearBeneficio_Throws_FormatException_On_InvalidEmail()
        {
            BeneficioModel beneficio = new();
            Assert.Throws<FormatException>(() => _beneficiosHandler.CrearBeneficio(beneficio, "correo.com"));
        }

        [Test]
        public void CrearBeneficio_Throws_KeyNotFoundException_On_Email_For_NonExistentCedula()
        {
            BeneficioModel beneficio = new();
            var correo = "correoSinEmpresa@gmail.com";
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns("");

            Assert.Throws<KeyNotFoundException>(() => _beneficiosHandler.CrearBeneficio(beneficio, correo));
        }

        [Test]
        public void CrearBeneficio_Throws_KeyNotFoundException_On_Email_For_NonExistentUsuario()
        {
            BeneficioModel beneficio = new();
            var correo = "correoConEmpresa@gmail.com";
            var cedulaJuridica = "1-234-567890";
            var idUsuario = -1;
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns(cedulaJuridica);
            _mockRepo.Setup(r => r.ObtenerIdUsuario(correo)).Returns(idUsuario);

            Assert.Throws<KeyNotFoundException>(() => _beneficiosHandler.CrearBeneficio(beneficio, correo));
        }

        [Test]
        public void CrearBeneficio_Throws_ResourceAlreadyExistsException_On_Registered_Beneficio()
        {
            BeneficioModel beneficio = new();
            beneficio.Nombre = "Nombre1";
            beneficio.Id = 1;
            beneficio.Descripcion = "Descripcion";
            beneficio.Tipo = "Tipo";
            beneficio.MesesMinimos = 0;
            beneficio.CantidadParametros = 1;
            beneficio.CedulaEmpresa = "1-234-567890";
            beneficio.Parametros = new();
            var correo = "correoConEmpresa@gmail.com";
            var cedulaJuridica = "1-234-567890";
            var idUsuario = 1;
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns(cedulaJuridica);
            _mockRepo.Setup(r => r.ObtenerIdUsuario(correo)).Returns(idUsuario);
            _mockRepo.Setup(r => r.ExisteBeneficio(cedulaJuridica, beneficio.Nombre)).Returns(true);

            Assert.Throws<ResourceAlreadyExistsException>(() => _beneficiosHandler.CrearBeneficio(beneficio, correo));
        }
    }
}