using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class GetDeduccionBeneficiosQuery : IGetDeduccionBeneficiosQuery
    {
        private readonly IEmpleadoRepository _repo;

        public GetDeduccionBeneficiosQuery(IEmpleadoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<DeduccionCalculada>> ExecuteAsync(string cedulaEmpleado)
        {
            var salarioBruto = await _repo.ObtenerSalarioBruto(cedulaEmpleado);
            var beneficios = await _repo.ObtenerBeneficiosEmpleado(cedulaEmpleado);

            var agrupados = beneficios
                .GroupBy(b => new { b.IDBeneficio, b.Nombre, b.Tipo })
                .Select(grupo =>
                {
                    decimal montoTotal = grupo.Sum(p => p.Monto);
                    decimal deduccion = 0;

                    if (grupo.Key.Tipo.ToLower() == "porcentaje")
                    {
                        deduccion = salarioBruto * (montoTotal / 100);
                    }
                    else if (grupo.Key.Tipo.ToLower() == "fijo" || grupo.Key.Tipo.ToLower() == "montofijo")
                    {
                        deduccion = montoTotal;
                    }

                    if (deduccion > salarioBruto || deduccion < 0)
                        deduccion = 0;

                    return new DeduccionCalculada
                    {
                        NombreBeneficio = grupo.Key.Nombre,
                        MontoReducido = deduccion
                    };
                })
                .ToList();

            return agrupados;
        }
    }

}
