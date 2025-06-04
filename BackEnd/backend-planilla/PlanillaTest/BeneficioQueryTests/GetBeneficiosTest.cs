using Moq;
using backend_planilla.Application;
using backend_planilla.Infraestructure;
using backend_planilla.Domain;

namespace PlanillaTest.BeneficioQueryTests
{
    public class GetBeneficiosTest
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
        public void GetBeneficios_Throws_FormatException_On_EmptyEmail()
        {
            Assert.Throws<FormatException>(() => _beneficiosHandler.GetBeneficios(""));
        }

        [Test]
        public void GetBeneficios_Throws_FormatException_On_InvalidEmail()
        {
            Assert.Throws<FormatException>(() => _beneficiosHandler.GetBeneficios("correo.com"));
        }

        [Test]
        public void GetBeneficios_Throws_KeyNotFoundException_On_Email_For_NonExistentCedula()
        {
            var correo = "correoSinEmpresa@gmail.com";
            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns("");

            Assert.Throws<KeyNotFoundException>(() => _beneficiosHandler.GetBeneficios(correo));
        }

        [Test]
        public void GetBeneficios_Returns_BeneficiosList_On_ValidEmail_And_ExistingCedula()
        {
            var correo = "correoConEmpresa@gmail.com";
            var cedula = "1-234-567890";
            var beneficios = new List<BeneficioModel>
            {
                new BeneficioModel { Nombre = "NoSeQueNombreUsar", Tipo = "Porcentaje" },
                new BeneficioModel { Nombre = "NoSeQueNombreUsar2", Tipo = "Fijo" },
            };

            _mockRepo.Setup(r => r.ObtenerCedulaJuridica(correo)).Returns(cedula);
            _mockRepo.Setup(r => r.ObtenerBeneficios(cedula)).Returns(beneficios);

            var resultadoBeneficios = _beneficiosHandler.GetBeneficios(correo);

            Assert.IsNotNull(resultadoBeneficios);
            Assert.IsInstanceOf<List<BeneficioModel>>(resultadoBeneficios);
            Assert.AreEqual(2, resultadoBeneficios.Count);
            Assert.AreEqual("NoSeQueNombreUsar", resultadoBeneficios[0].Nombre);
            Assert.AreEqual("Porcentaje", resultadoBeneficios[0].Tipo);
            Assert.AreEqual("NoSeQueNombreUsar2", resultadoBeneficios[1].Nombre);
            Assert.AreEqual("Fijo", resultadoBeneficios[1].Tipo);
        }
    }
}