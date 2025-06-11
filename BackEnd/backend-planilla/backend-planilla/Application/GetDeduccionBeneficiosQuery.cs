using System.Text.Json;
using System.Text;
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

            var agrupados = await Task.WhenAll(beneficios
                .GroupBy(b => new { b.IDBeneficio, b.Nombre, b.Tipo, b.Descripción })
                .Select(async grupo =>
                {
                decimal deduccion = 0;
                decimal montoTotal = grupo.Sum(p => p.Monto);

                if (grupo.Key.Tipo.ToLower() == "porcentaje")
                {
                    deduccion = salarioBruto * (montoTotal / 100);
                }
                else if (grupo.Key.Tipo.ToLower() == "fijo" || grupo.Key.Tipo.ToLower() == "montofijo")
                {
                    deduccion = montoTotal;
                }
                else if (grupo.Key.Tipo.ToLower() == "api")
                {
                        if (grupo.Key.Descripción.ToLower() == "poliza de vida")
                        {
                            var sexo = await _repo.ObtenerGeneroEmpleado(cedulaEmpleado);
                            deduccion = await ObtenerDeduccionPolizaVida(sexo);
                        }
                        else if (grupo.Key.Descripción.ToLower() == "asociación solidarista") {
                            deduccion = await ObtenerDeduccionAsociacion(salarioBruto, "Asociación");
                        }
                        else if (grupo.Key.Descripción.ToLower() == "seguro medico")
                        {
                            deduccion = await ObtenerDeduccionMediSeguro();
                        }

                    }

                if (deduccion > salarioBruto || deduccion < 0)
                    deduccion = 0;

                return new DeduccionCalculada
                {
                    NombreBeneficio = grupo.Key.Nombre,
                    MontoReducido = deduccion
                };
            })
);

            return agrupados.ToList();
        }

        private async Task<decimal> ObtenerDeduccionPolizaVida(string sexo)
        {
            var fechaNacimiento = "2000-01-01"; // constante para pruebas
            var url = $"https://poliza-friends-grg0h9g5crf2hwh8.southcentralus-01.azurewebsites.net/api/LifeInsurance" +
                      $"?dateofbirth={fechaNacimiento}&sex={sexo}";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("FRIENDS-API-TOKEN",
                "1D0B194488C091852597B9AF7D1AA8F23D55C9784815489CF0A488B6F2C6D5C4C569AD51231FACB9920E5A763FE388247E03131D1601AC234E86BC0D266EB6A7");

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var contenido = await response.Content.ReadAsStringAsync();
            return decimal.TryParse(contenido, out var resultado) ? resultado : 0;
        }

        private async Task<decimal> ObtenerDeduccionAsociacion(decimal salarioBruto, string nombreAsociacion)
        {
            using var httpClient = new HttpClient();
            var url = "https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate"; 

            var payload = new
            {
                nombreAsociacion = nombreAsociacion,
                salarioBruto = salarioBruto
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.Add("API-KEY", "Tralalerotralala");

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (decimal.TryParse(result, out var deduccion))
                    return deduccion;
            }

            return 0; 
        }

        private async Task<decimal> ObtenerDeduccionMediSeguro()
        {
            using var httpClient = new HttpClient();

            var url = "https://mediseguro-vorlagenersteller-d4hmbvf7frg7aqan.southcentralus-01.azurewebsites.net/api/MediSeguroMonto";

            var requestData = new
            {
                fechaNacimiento = "1990-05-07",
                genero = "femenino",
                cantidadDependientes = 2
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Add("token", "TOKEN123");

            var response = await httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (decimal.TryParse(result, out var deduccion))
                    return deduccion;
            }

            return 0;
        }
    }

}
