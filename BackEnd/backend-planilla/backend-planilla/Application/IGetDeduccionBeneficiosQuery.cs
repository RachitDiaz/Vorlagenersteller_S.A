using System.Collections.Generic;
using System.Threading.Tasks;
using backend_planilla.Application;
using backend_planilla.Domain;

public interface IGetDeduccionBeneficiosQuery
{
    public Task<DeduccionCalculadaConTotal> CalcularDeduccionesBeneficios(string cedula);

}