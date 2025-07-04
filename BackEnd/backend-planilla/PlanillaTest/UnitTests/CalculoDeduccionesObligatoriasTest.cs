using backend_planilla.Application;
using backend_planilla.Domain;
using NUnit.Framework;

namespace PlanillaTest.UnitTests
{
    public class CalculoDeduccionesObligatoriasTest
    {
        private ICalculoDeduccionesObligatorias _calculadora;

        [SetUp]
        public void Setup()
        {
            _calculadora = new CalculoDeduccionesObligatorias();
        }

        [Test]
        public void DeduccionesCorrectasParaSalarioValidoTest()
        {
            decimal salario = 1000000m;
            var resultado = _calculadora.CalcularDeduccionMensual(salario);

            Assert.That(resultado.SEMEmpleado, Is.EqualTo(55000m));
            Assert.That(resultado.IVMEmpleado, Is.EqualTo(41700m));
            Assert.That(resultado.BPPOEmpleado, Is.EqualTo(10000m));

            Assert.That(resultado.SEMPatrono, Is.EqualTo(92500m));
            Assert.That(resultado.IVMPatrono, Is.EqualTo(54200m));
            Assert.That(resultado.FCLPatrono, Is.EqualTo(15000m));
        }

        [Test]
        public void DeduccionesEnCeroParaSalarioCeroTest()
        {
            var resultado = _calculadora.CalcularDeduccionMensual(0m);

            Assert.That(resultado.TotalEmpleado, Is.EqualTo(0));
            Assert.That(resultado.TotalPatrono, Is.EqualTo(0));
        }

        [Test]
        public void TotalesCalculadosCorrectamenteTest()
        {
            decimal salario = 1500000m;
            var r = _calculadora.CalcularDeduccionMensual(salario);

            decimal esperadoEmpleado = r.SEMEmpleado + r.IVMEmpleado + r.BPPOEmpleado + r.ImpuestoRenta;
            decimal esperadoPatrono = r.SEMPatrono + r.IVMPatrono + r.BPOPPatrono + r.AsignacionesFamiliaresPatrono + r.IMASPatrono + r.INAPatrono + r.FCLPatrono + r.OPCPatrono + r.INSPatrono;

            Assert.That(r.TotalEmpleado, Is.EqualTo(esperadoEmpleado));
            Assert.That(r.TotalPatrono, Is.EqualTo(esperadoPatrono));
        }

        [Test]
        public void DeduccionesQuincenalesCorrectasTest()
        {
            decimal salario = 1000000m;

            var mensual = _calculadora.CalcularDeduccionMensual(salario);
            var quincenal = _calculadora.CalcularDeduccionQuincenal(salario);

            Assert.That(quincenal.SEMEmpleado, Is.EqualTo(mensual.SEMEmpleado / 2));
            Assert.That(quincenal.IVMEmpleado, Is.EqualTo(mensual.IVMEmpleado / 2));
            Assert.That(quincenal.BPPOEmpleado, Is.EqualTo(mensual.BPPOEmpleado / 2));
            Assert.That(quincenal.ImpuestoRenta, Is.EqualTo(mensual.ImpuestoRenta / 2));
        }
    }
}


