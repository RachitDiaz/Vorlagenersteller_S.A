using backend_planilla.Domain;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class GenerarCalculosQuery
    {
        private readonly IGenerarCalculosRepository _repository;
        private readonly IGetDeduccionBeneficiosQuery _query;

        public GenerarCalculosQuery(IGenerarCalculosRepository repository, IGetDeduccionBeneficiosQuery query)
        {
            _repository = repository;
            _query = query;
        }

        public async Task<List<ResultadoEmpleadoModel>> ObtenerResultadosAsync(string cedulaJuridica, string tipoPlanilla, ICalculoDeduccionesObligatorias calculadora, IGetDeduccionBeneficiosQuery beneficios)
        {
            var empleados = await _repository.ObtenerEmpleadosConSalarioAsync(cedulaJuridica);
            var resultados = new List<ResultadoEmpleadoModel>();
            var quincenasEnUnMes = 2;
            var esQuincenal = tipoPlanilla.ToLower() == "quincenal";

            foreach (var empleado in empleados)
            {
                var deducciones = esQuincenal
                    ? calculadora.CalcularDeduccionQuincenal(empleado.Salario)
                    : calculadora.CalcularDeduccionMensual(empleado.Salario);

                var salarioBrutoParaGuardar = esQuincenal
                    ? empleado.Salario / quincenasEnUnMes
                    : empleado.Salario;

                var resultadoBeneficios = await _query.CalcularDeduccionesBeneficios(empleado.CedulaEmpleado);

                if (esQuincenal)
                {
                    foreach (var beneficio in resultadoBeneficios.DeduccionesCalculadas)
                    {
                        beneficio.MontoReducido /= quincenasEnUnMes;
                    }

                    resultadoBeneficios.Total /= quincenasEnUnMes;
                }

                resultados.Add(new ResultadoEmpleadoModel
                {
                    CedulaEmpleado = empleado.CedulaEmpleado,
                    SalarioBruto = salarioBrutoParaGuardar,
                    Deducciones = deducciones,
                    Beneficios = resultadoBeneficios
                });
            }

            return resultados;
        }
    }
}
