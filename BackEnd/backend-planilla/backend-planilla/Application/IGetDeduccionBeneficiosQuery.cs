using System.Collections.Generic;
using System.Threading.Tasks;
using backend_planilla.Application;

public interface IGetDeduccionBeneficiosQuery
{
    public Task<List<DeduccionCalculada>> CalcularDeduccioensBeneficios(string correo);


}