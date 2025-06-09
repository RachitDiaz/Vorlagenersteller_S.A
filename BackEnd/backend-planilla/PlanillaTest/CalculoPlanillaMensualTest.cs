using backend_planilla.Application;
using backend_planilla.Domain;
using NUnit.Framework;

namespace PlanillaTest
{
    public class CalculoPlanillaMensualTest
    {
        private CalcularDeduccionesPlanilla _calculadora;

        [SetUp]
        public void Setup()
        {
            _calculadora = new CalcularDeduccionesPlanilla();
        }

        [Test]
        public void DeduccionesCorrectasParaSalarioValidoTest()
        {
            decimal salario = 1000000m;

            var resultado = _calculadora.Calcular(salario);

            Assert.That(resultado.SEMEmpleado, Is.EqualTo(55000m));
            Assert.That(resultado.IVMEmpleado, Is.EqualTo(41700m));
            Assert.That(resultado.LPTEmpleado, Is.EqualTo(10000m));

            Assert.That(resultado.SEMPatrono, Is.EqualTo(92500m));
            Assert.That(resultado.IVMPatrono, Is.EqualTo(54200m));
            Assert.That(resultado.LPTPatrono, Is.EqualTo(47500m));
        }

        [Test]
        public void DeduccionesEnCeroParaSalarioCeroTest()
        {
            var resultado = _calculadora.Calcular(0m);

            Assert.That(resultado.TotalEmpleado, Is.EqualTo(0));
            Assert.That(resultado.TotalPatrono, Is.EqualTo(0));
        }

        [Test]
        public void TotalesCalculadosCorrectamenteTest()
        {
            decimal salario = 1500000m;
            var r = _calculadora.Calcular(salario);

            decimal esperadoEmpleado = r.SEMEmpleado + r.IVMEmpleado + r.LPTEmpleado + r.ImpuestoRenta;
            decimal esperadoPatrono = r.SEMPatrono + r.IVMPatrono + r.LPTPatrono;

            Assert.That(r.TotalEmpleado, Is.EqualTo(esperadoEmpleado));
            Assert.That(r.TotalPatrono, Is.EqualTo(esperadoPatrono));
        }
    }
}

