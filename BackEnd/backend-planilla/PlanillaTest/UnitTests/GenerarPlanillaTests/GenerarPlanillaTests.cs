using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend_planilla.Application;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Moq;

namespace PlanillaTest.UnitTests.GenerarPlanillaTests
{
    public class GenerarPlanillaTests
    {
        private Mock<IPlanillaRepository> _repoMock = null!;
        private Mock<ICalculoDeduccionesObligatorias> _calcMock = null!;
        private Mock<IGetDeduccionBeneficiosQuery> _beneficioMock = null!;
        private Mock<IGenerarCalculosQuery> _calculosQueryMock = null!;
        private GenerarPlanilla _servicio = null!;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IPlanillaRepository>();
            _calcMock = new Mock<ICalculoDeduccionesObligatorias>();
            _beneficioMock = new Mock<IGetDeduccionBeneficiosQuery>();
            _calculosQueryMock = new Mock<IGenerarCalculosQuery>();
            _servicio = new GenerarPlanilla(_repoMock.Object, _calculosQueryMock.Object);
        }

        [Test]
        public void EjecutarAsync_PlanillaExistente_DeberiaLanzarExcepcion()
        {
            var request = new GenerarPlanillaRequestModel
            {
                CedulaJuridica = "123"
            };

            var periodo = GenerarPlanilla.GenerarPeriodo("mensual");

            _repoMock.Setup(r => r.GetTipoDePagoAsync("123")).ReturnsAsync("mensual");
            _repoMock.Setup(r => r.ExistePeriodoAsync("123", periodo)).ReturnsAsync(true);

            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _servicio.EjecutarAsync(request, _calcMock.Object, _beneficioMock.Object));
        }

        [Test]
        public async Task EjecutarAsync_CasoValido_DeberiaRetornarIdPlanilla()
        {
            var cedula = "123";
            var request = new GenerarPlanillaRequestModel { CedulaJuridica = cedula };
            var periodo = GenerarPlanilla.GenerarPeriodo("mensual");
            var resultado = new List<ResultadoEmpleadoModel>
    {
        new ResultadoEmpleadoModel
        {
            CedulaEmpleado = "1",
            SalarioBruto = 1000,
            Deducciones = new DeduccionesObligatoriasModel(),
            Beneficios = new DeduccionCalculadaConTotal()
        }
    };

            _repoMock.Setup(r => r.GetTipoDePagoAsync(cedula)).ReturnsAsync("mensual");
            _repoMock.Setup(r => r.ExistePeriodoAsync(cedula, periodo)).ReturnsAsync(false);
            _calculosQueryMock.Setup(q =>
                q.ObtenerResultadosAsync(cedula, "mensual", _calcMock.Object, _beneficioMock.Object))
                .ReturnsAsync(resultado);

            var expectedId = Guid.NewGuid();
            _repoMock.Setup(r => r.InsertarPlanillaCompletaAsync(
                cedula, periodo, It.IsAny<DateTime>(), resultado, "mensual"))
                .ReturnsAsync(expectedId);

            var idPlanilla = await _servicio.EjecutarAsync(request, _calcMock.Object, _beneficioMock.Object);

            Assert.That(idPlanilla, Is.EqualTo(expectedId));
        }
    }
}
