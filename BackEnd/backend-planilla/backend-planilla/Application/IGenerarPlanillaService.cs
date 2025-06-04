namespace backend_planilla.Application
{
    public interface IGenerarPlanillaService
    {
        Task<bool> GenerarAsync(string cedulaEmpresa, int idPlanilla);
    }
}
