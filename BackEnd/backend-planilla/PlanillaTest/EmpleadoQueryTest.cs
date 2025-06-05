using backend_planilla.Infraestructure;
using backend_planilla.Application;
using backend_planilla.Domain;
using Moq;
using backend_planilla.Models;

namespace PlanillaTest
{
    public class EmpleadoQueryTest
    {
        private Mock<IEmpleadoRepository> _mockRepo;
        private IEmpleadoQuery _empleadoQuery;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IEmpleadoRepository>();
            _empleadoQuery = new EmpleadoQuery(_mockRepo.Object);
        }

        [Test]
        public void EdicionValida()
        {
            string cedulaEmpleado = "1-2222-3333";
            InfoEmpleadoModel entrada = new InfoEmpleadoModel();
            InfoEmpleadoModel original = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2314-1241",
                    CedulaEmpresa = "",
                    Nombre = "", 
                    Apellido1 = "",
                    Apellido2 = "",
                    Banco = "",
                    SalarioBruto = 1,
                    TipoContrato = ""
                },
                Genero = "",
                Correo = "",
                CedulaEditable = true
            };

            bool respuesta = true;
            bool resultadoEsperado = true;

            _mockRepo.Setup(repo => repo.ObtenerInfoEmpleado(cedulaEmpleado)).Returns(original);
            _mockRepo.Setup(repo => repo.EditarInfoEmpleado(entrada, cedulaEmpleado)).Returns(respuesta);

            var resultado = _empleadoQuery.EditarInfoEmpleado(entrada, cedulaEmpleado);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }
    }
}