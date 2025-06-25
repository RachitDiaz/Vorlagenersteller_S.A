using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public interface ICalculoDeduccionesObligatorias
    {
        DeduccionesObligatoriasModel CalcularDeduccionMensual(decimal salarioBruto);
        DeduccionesObligatoriasModel CalcularDeduccionQuincenal(decimal salarioBruto);
    }
}
