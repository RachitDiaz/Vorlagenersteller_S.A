using System.Collections.Generic;
using System.Threading.Tasks;
using backend_planilla.Application;
using backend_planilla.Domain;

public interface IGetDeduccionBeneficiosQuery
{
    public Task<List<DeduccionCalculada>> CalcularDeduccionesBeneficios(string cedula);

}