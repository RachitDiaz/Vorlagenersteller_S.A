using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend_planilla.Application;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Moq;

namespace PlanillaTest.GenerarPlanillaTests
{
    public class GenerarPlanillaTests
    {
        private Mock<IPlanillaRepository> _repoMock = null!;
        private Mock<ICalculoDeduccionesObligatorias> _calcMock = null!;
        private Mock<IGetDeduccionBeneficiosQuery> _beneficioMock = null!;
        private Mock<GenerarCalculosQuery> _calculosQueryMock = null!;
        private GenerarPlanilla _servicio = null!;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IPlanillaRepository>();
            _calcMock = new Mock<ICalculoDeduccionesObligatorias>();
            _beneficioMock = new Mock<IGetDeduccionBeneficiosQuery>();
            _calculosQueryMock = new Mock<GenerarCalculosQuery>(MockBehavior.Strict, null!, null!);
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
    }
}
