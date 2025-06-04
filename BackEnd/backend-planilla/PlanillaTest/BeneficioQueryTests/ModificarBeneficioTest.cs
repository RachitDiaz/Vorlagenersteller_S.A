using Moq;
using backend_planilla.Application;
using backend_planilla.Infraestructure;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace PlanillaTest.BeneficioQueryTests
{
    public class ModificarBeneficioTest
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
        public void ModificarBeneficio_Throws_FormatException_On_EmptyEmail()
        {
            BeneficioModel viejo = new();
            BeneficioModel nuevo = new();
            Assert.Throws<FormatException>(() => _beneficiosHandler.ModificarBeneficio(nuevo, viejo, ""));
        }

        [Test]
        public void ModificarBeneficio_Throws_FormatException_On_InvalidEmail()
        {
            BeneficioModel viejo = new();
            BeneficioModel nuevo = new();
            Assert.Throws<FormatException>(() => _beneficiosHandler.ModificarBeneficio(nuevo, viejo, "correo.com"));
        }

        [Test]
        public void ModificarBeneficio_Throws_KeyNotFoundException_On_Email_For_NonExistentCedula()
        {
            BeneficioModel viejo = new();
            BeneficioModel nuevo = new();
            var correo = "correoSinEmpresa@gmail.com";
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns("");

            Assert.Throws<KeyNotFoundException>(() => _beneficiosHandler.ModificarBeneficio(nuevo, viejo, correo));
        }

        [Test]
        public void ModificarBeneficio_Throws_KeyNotFoundException_On_Email_For_NonExistentUsuario()
        {
            BeneficioModel viejo = new();
            BeneficioModel nuevo = new();
            var correo = "correoConEmpresa@gmail.com";
            var cedulaJuridica = "1-234-567890";
            var idUsuario = -1;
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns(cedulaJuridica);
            _mockRepo.Setup(r => r.ObtenerIdUsuario(correo)).Returns(idUsuario);

            Assert.Throws<KeyNotFoundException>(() => _beneficiosHandler.ModificarBeneficio(nuevo, viejo, correo));
        }

        [Test]
        public void ModificarBeneficio_Throws_InvalidOperationException_On_Beneficios_With_NoChanges()
        {
            BeneficioModel viejo = new();
            viejo.Nombre = "Nombre1";
            viejo.Id = 1;
            viejo.Descripcion = "Descripcion";
            viejo.Tipo = "Tipo";
            viejo.MesesMinimos = 0;
            viejo.CantidadParametros = 1;
            viejo.CedulaEmpresa = "1-234-567890";
            viejo.Parametros = new();
            BeneficioModel nuevo = viejo;
            var correo = "correoConEmpresa@gmail.com";
            var cedulaJuridica = "1-234-567890";
            var idUsuario = 1;
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns(cedulaJuridica);
            _mockRepo.Setup(r => r.ObtenerIdUsuario(correo)).Returns(idUsuario);

            Assert.Throws<InvalidOperationException>(() => _beneficiosHandler.ModificarBeneficio(nuevo, viejo, correo));
        }

        [Test]
        public void ModificarBeneficio_Throws_InvalidOperationException_On_UnmodifiableBeneficio()
        {
            BeneficioModel viejo = new();
            viejo.Nombre = "Nombre1";
            viejo.Id = 1;
            viejo.Descripcion = "Descripcion";
            viejo.Tipo = "Tipo";
            viejo.MesesMinimos = 0;
            viejo.CantidadParametros = 1;
            viejo.CedulaEmpresa = "1-234-567890";
            viejo.Parametros = new();
            BeneficioModel nuevo = viejo;
            nuevo.Nombre = "Nombre2";
            var correo = "correoConEmpresa@gmail.com";
            var cedulaJuridica = "1-234-567890";
            var idUsuario = 1;
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns(cedulaJuridica);
            _mockRepo.Setup(r => r.ObtenerIdUsuario(correo)).Returns(idUsuario);
            _mockRepo.Setup(r => r.SePuedeModificar(viejo.Id, cedulaJuridica)).Returns(false);

            Assert.Throws<InvalidOperationException>(() => _beneficiosHandler.ModificarBeneficio(nuevo, viejo, correo));
        }

        [Test]
        public void ModificarBeneficio_Returns_True_On_ValidData()
        {
            BeneficioModel viejo = new();
            viejo.Nombre = "Nombre1";
            viejo.Id = 1;
            viejo.Descripcion = "Descripcion";
            viejo.Tipo = "Tipo";
            viejo.MesesMinimos = 0;
            viejo.CantidadParametros = 1;
            viejo.CedulaEmpresa = "1-234-567890";
            viejo.Parametros = new();
            BeneficioModel nuevo = new BeneficioModel
            {
                Id = viejo.Id,
                Nombre = "Nombre2",
                Descripcion = viejo.Descripcion,
                Tipo = viejo.Tipo,
                MesesMinimos = viejo.MesesMinimos,
                CantidadParametros = viejo.CantidadParametros,
                CedulaEmpresa = viejo.CedulaEmpresa,
                Parametros = new List<ParametroBeneficioModel>(viejo.Parametros)
            };
            var correo = "correoConEmpresa@gmail.com";
            var cedulaJuridica = "1-234-567890";
            var idUsuario = 1;
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns(cedulaJuridica);
            _mockRepo.Setup(r => r.ObtenerIdUsuario(correo)).Returns(idUsuario);
            _mockRepo.Setup(r => r.SePuedeModificar(viejo.Id, cedulaJuridica)).Returns(true);
            _mockRepo.Setup(r => r.ModificarBeneficio(nuevo, idUsuario)).Returns(true);
            _mockRepo.Setup(r => r.ModificarParametros(nuevo.Parametros, idUsuario)).Returns(true);

            var resultado = _beneficiosHandler.ModificarBeneficio(nuevo, viejo, correo);

            Assert.IsTrue(resultado);
        }
    }
}