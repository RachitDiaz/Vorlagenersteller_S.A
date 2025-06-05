using System.Collections.Generic;
using System.Threading.Tasks;
using backend_planilla.Application;

public interface IGetDeduccionBeneficiosQuery
{
    Task<List<DeduccionCalculada>> ExecuteAsync(string cedulaEmpleado);
}