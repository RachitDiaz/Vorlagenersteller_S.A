using backend_planilla.Domain;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class GenerarCalculosQuery
    {
        private readonly IGenerarCalculosRepository _repository;

        public GenerarCalculosQuery(IGenerarCalculosRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ResultadoEmpleadoModel>> ObtenerResultadosAsync(string cedulaJuridica, string tipoPlanilla, ICalculoDeduccionesObligatorias calculadora)
        {
            var empleados = await _repository.ObtenerEmpleadosConSalarioAsync(cedulaJuridica);
            var resultados = new List<ResultadoEmpleadoModel>();
            var quincenasEnUnMes = 2;

            foreach (var empleado in empleados)
            {
                var deducciones = tipoPlanilla.ToLower() == "quincenal"
                    ? calculadora.CalcularDeduccionQuincenal(empleado.Salario)
                    : calculadora.CalcularDeduccionMensual(empleado.Salario);

                var salarioBrutoParaGuardar = tipoPlanilla.ToLower() == "quincenal"
                    ? empleado.Salario / quincenasEnUnMes
                    : empleado.Salario;

                resultados.Add(new ResultadoEmpleadoModel
                {
                    CedulaEmpleado = empleado.CedulaEmpleado,
                    SalarioBruto = salarioBrutoParaGuardar,
                    Deducciones = deducciones
                });
            }

            return resultados;
        }
    }
}
