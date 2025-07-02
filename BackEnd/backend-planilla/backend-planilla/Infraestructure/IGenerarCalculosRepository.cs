namespace backend_planilla.Infraestructure
{
    public interface IGenerarCalculosRepository
    {
        Task<List<(string CedulaEmpleado, decimal Salario)>> ObtenerEmpleadosConSalarioAsync(string cedulaJuridica);
    }
}
