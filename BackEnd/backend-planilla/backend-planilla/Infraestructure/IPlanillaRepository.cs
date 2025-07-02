using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IPlanillaRepository
    {
        Task<Guid> InsertarPlanillaEmpresaAsync(string cedulaJuridica, string periodo, DateTime fecha);
        Task InsertarPlanillaEmpleadoAsync(Guid idPlanilla, ResultadoEmpleadoModel empleado);
        Task<Guid> InsertarPlanillaCompletaAsync(string cedulaJuridica, string periodo, DateTime fechaGeneracion, List<ResultadoEmpleadoModel> empleados, string tipo);
        Task<bool> ExistePeriodoAsync(string cedulaJuridica, string periodo);
        Task<string> GetTipoDePagoAsync(string cedulaJuridica);
    }
}
