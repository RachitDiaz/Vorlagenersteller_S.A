﻿using backend_planilla.Application;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Moq;
using NUnit.Framework;

namespace PlanillaTest.UnitTests
{
    public class BeneficioQueryTest
    {
        private Mock<IBeneficioRepository> _mockRepo;
        private BeneficioQuery _query;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IBeneficioRepository>();

            _query = new BeneficioQuery(_mockRepo.Object);
        }

        [Test]
        public void ActualizarDependientesEmpleado_DebeLlamarAlRepoCorrectamente()
        {
            string correo = "test@email.com";
            string cedula = "1-2345-6789";
            int dependientes = 2;

            _mockRepo.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns(cedula);
            _mockRepo.Setup(r => r.ActualizarDependientesEmpleado(cedula, dependientes)).Returns(true);

            var resultado = _query.ActualizarDependientesEmpleado(correo, dependientes);

            Assert.IsTrue(resultado);
            _mockRepo.Verify(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo), Times.Once);
            _mockRepo.Verify(r => r.ActualizarDependientesEmpleado(cedula, dependientes), Times.Once);
        }

        [Test]
        public void ActualizarDependientesEmpleado_CedulaNoExiste_RetornaFalse()
        {
            string correo = "noexiste@email.com";
            int dependientes = 3;

            _mockRepo.Setup(r => r.ObtenerCedulaEmpleadoDesdeCorreo(correo)).Returns((string)null);

            var resultado = _query.ActualizarDependientesEmpleado(correo, dependientes);

            Assert.IsFalse(resultado);
            _mockRepo.Verify(r => r.ActualizarDependientesEmpleado(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}
