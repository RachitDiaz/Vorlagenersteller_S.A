using Moq;
using backend_planilla.Application;
using backend_planilla.Infraestructure;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using backend_planilla.Exceptions;
using backend_planilla.Handlers;

namespace PlanillaTest.EmpleadoQueryTests
{
    class EliminarEmpleadoTest
    {
        private EmpleadoQuery _empleadoQuery;
        private Mock<IEmpleadoRepository> _mockRepo;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IEmpleadoRepository>();
            _empleadoQuery = new EmpleadoQuery(_mockRepo.Object);
        }

        [Test]
        public void EliminarEmpleado_Throws_InvalidOperationException_On_NonExistingEmpleado()
        {
            string cedulaSinEmpleado = "NoHayEmpleado";
            _mockRepo.Setup(r => r.EliminarEmpleado(cedulaSinEmpleado)).Throws(new InvalidOperationException());
            Assert.Throws<InvalidOperationException>(() => _empleadoQuery.EliminarEmpleado(cedulaSinEmpleado));
        }

        [Test]
        public void EliminarEmpleado_Throws_Exception_On_UnexpectedError_In_Query()
        {
            string cedulaSinEmpleado = "NoHayEmpleado";
            _mockRepo.Setup(r => r.EliminarEmpleado(cedulaSinEmpleado)).Throws(new Exception());
            Assert.Throws<Exception>(() => _empleadoQuery.EliminarEmpleado(cedulaSinEmpleado));
        }

        [Test]
        public void EliminarEmpleado_Returns_True_On_ValidEmail()
        {
            string cedulaConEmpleado = "TengoEmpleado";
            _mockRepo.Setup(r => r.EliminarEmpleado(cedulaConEmpleado)).Returns("tengo.empleado@gmail.com");

            var resultadoEliminar = _empleadoQuery.EliminarEmpleado(cedulaConEmpleado);

            Assert.IsTrue(resultadoEliminar);
        }
    }
}
