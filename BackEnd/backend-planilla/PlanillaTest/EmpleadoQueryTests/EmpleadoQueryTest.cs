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
            InfoEmpleadoModel entrada = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2353-1241",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Gerardo",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 300000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto400@gmail.com",
                CedulaEditable = true
            };
            InfoEmpleadoModel original = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2222-3333",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Alberto", 
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 10000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto313@gmail.com",
                CedulaEditable = true
            };

            bool respuesta = true;
            bool resultadoEsperado = true;

            _mockRepo.Setup(repo => repo.ObtenerInfoEmpleado(cedulaEmpleado)).Returns(original);
            _mockRepo.Setup(repo => repo.EditarInfoEmpleado(entrada, cedulaEmpleado)).Returns(respuesta);

            var resultado = _empleadoQuery.EditarInfoEmpleado(entrada, cedulaEmpleado);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void NoEditable()
        {
            string cedulaEmpleado = "1-2222-3333";
            InfoEmpleadoModel entrada = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2353-1241",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Gerardo",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 300000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto400@gmail.com",
                CedulaEditable = false
            };
            InfoEmpleadoModel original = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2222-3333",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Alberto",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 10000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto313@gmail.com",
                CedulaEditable = false
            };

            bool respuesta = true;
            bool resultadoEsperado = false;

            _mockRepo.Setup(repo => repo.ObtenerInfoEmpleado(cedulaEmpleado)).Returns(original);
            _mockRepo.Setup(repo => repo.EditarInfoEmpleado(entrada, cedulaEmpleado)).Returns(respuesta);

            var resultado = _empleadoQuery.EditarInfoEmpleado(entrada, cedulaEmpleado);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void NoEditableValido()
        {
            string cedulaEmpleado = "1-2222-3333";
            InfoEmpleadoModel entrada = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2222-3333",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Alberto",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 300000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto400@gmail.com",
                CedulaEditable = false
            };
            InfoEmpleadoModel original = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2222-3333",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Alberto",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 10000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto313@gmail.com",
                CedulaEditable = false
            };

            bool respuesta = true;
            bool resultadoEsperado = true;

            _mockRepo.Setup(repo => repo.ObtenerInfoEmpleado(cedulaEmpleado)).Returns(original);
            _mockRepo.Setup(repo => repo.EditarInfoEmpleado(entrada, cedulaEmpleado)).Returns(respuesta);

            var resultado = _empleadoQuery.EditarInfoEmpleado(entrada, cedulaEmpleado);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void NoEditableInvalido()
        {
            string cedulaEmpleado = "1-2222-3333";
            InfoEmpleadoModel entrada = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2122-3773",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Fernando",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 300000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto400@gmail.com",
                CedulaEditable = false
            };
            InfoEmpleadoModel original = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2222-3333",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Alberto",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 10000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto313@gmail.com",
                CedulaEditable = false
            };

            bool respuesta = true;
            bool resultadoEsperado = false;

            _mockRepo.Setup(repo => repo.ObtenerInfoEmpleado(cedulaEmpleado)).Returns(original);
            _mockRepo.Setup(repo => repo.EditarInfoEmpleado(entrada, cedulaEmpleado)).Returns(respuesta);

            var resultado = _empleadoQuery.EditarInfoEmpleado(entrada, cedulaEmpleado);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }

        [Test]
        public void EditableDiscrepante()
        {
            string cedulaEmpleado = "1-2222-3333";
            InfoEmpleadoModel entrada = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2222-3333",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Gerardo",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 300000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto400@gmail.com",
                CedulaEditable = true
            };
            InfoEmpleadoModel original = new InfoEmpleadoModel()
            {
                Empleado = new EmpleadoModel
                {
                    CedulaEmpleado = "1-2222-3333",
                    CedulaEmpresa = "1-318-013913",
                    Nombre = "Alberto",
                    Apellido1 = "Morales",
                    Apellido2 = "Torrez",
                    Banco = "BN1294019",
                    SalarioBruto = 10000,
                    TipoContrato = "Tiempo Completo"
                },
                Genero = "Masculino",
                Correo = "alberto313@gmail.com",
                CedulaEditable = false
            };

            bool respuesta = true;
            bool resultadoEsperado = false;

            _mockRepo.Setup(repo => repo.ObtenerInfoEmpleado(cedulaEmpleado)).Returns(original);
            _mockRepo.Setup(repo => repo.EditarInfoEmpleado(entrada, cedulaEmpleado)).Returns(respuesta);

            var resultado = _empleadoQuery.EditarInfoEmpleado(entrada, cedulaEmpleado);

            Assert.That(resultadoEsperado, Is.EqualTo(resultado));
        }
    }
}