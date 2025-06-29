using System.Text.Json;
using System.Text;
using backend_planilla.Infraestructure;
using backend_planilla.Domain;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Azure;

namespace backend_planilla.Application
{
    public class GetDeduccionBeneficiosQuery : IGetDeduccionBeneficiosQuery
    {
        private readonly IEmpleadoRepository _repo_empleado;
        private readonly IBeneficiosRepository _repo_beneficios;
        private readonly IBeneficioRepository _repo_beneficio;

        public GetDeduccionBeneficiosQuery(IEmpleadoRepository repo_empleado, IBeneficiosRepository repo_beneficios, IBeneficioRepository repo_beneficio)
        {
            _repo_empleado = repo_empleado;
            _repo_beneficios = repo_beneficios;
            _repo_beneficio = repo_beneficio;
        }


        public async Task<List<DeduccionCalculada>> CalcularDeduccionesBeneficios(string correo)
        {
            string cedulaEmpleado = _repo_beneficio.ObtenerCedulaEmpleadoDesdeCorreo(correo);
            decimal salarioBruto = await _repo_empleado.ObtenerSalarioBruto(cedulaEmpleado);
            var beneficios = await _repo_empleado.ObtenerBeneficiosEmpleado(cedulaEmpleado);

            var resultado = new List<DeduccionCalculada>();

            foreach (var beneficio in beneficios)
            {
                decimal deduccion = await CalcularDeduccionPorBeneficio(beneficio, salarioBruto, cedulaEmpleado);

                deduccion = Math.Clamp(deduccion, 0, salarioBruto);

                resultado.Add(new DeduccionCalculada
                {
                    NombreBeneficio = beneficio.Nombre,
                    MontoReducido = Math.Round(deduccion, 2)
                });
            }

            while (resultado.Count < 3)
            {
                resultado.Add(new DeduccionCalculada
                {
                    NombreBeneficio = "Sin beneficio",
                    MontoReducido = 0
                });
            }

            return resultado;
        }

        private async Task<decimal> CalcularDeduccionPorBeneficio(DeduccionBeneficioModel beneficio, decimal salarioBruto, string cedulaEmpleado)
        {
            string tipo = beneficio.Tipo.ToLower();
            string nombre = beneficio.Nombre.ToLower();

            switch (tipo)
            {
                case "porcentaje":
                    return await CalcularPorcentaje(beneficio.IDBeneficio, salarioBruto);

                case "fijo":
                case "montofijo":
                    return await CalcularFijo(beneficio.IDBeneficio);

                case "api":
                    return await CalcularDesdeApi(nombre, salarioBruto, cedulaEmpleado);

                default:
                    return 0;
            }
        }

        private async Task<decimal> CalcularPorcentaje(int idBeneficio, decimal salarioBruto)
        {
            const decimal cien = 100m;
            var parametros = _repo_beneficios.ObtenerParametrosBeneficio(idBeneficio);
            return parametros
                .Where(p => p.TipoValorParametro.Equals("porcentaje", StringComparison.OrdinalIgnoreCase))
                .Sum(p => salarioBruto * (p.ValorDelParametro / cien));
        } 

        private async Task<decimal> CalcularFijo(int idBeneficio)
        {
            var parametros =  _repo_beneficios.ObtenerParametrosBeneficio(idBeneficio);
            return parametros
                .Where(p => p.TipoValorParametro.Equals("fijo", StringComparison.OrdinalIgnoreCase))
                .Sum(p => p.ValorDelParametro);
        }

        private async Task<decimal> CalcularDesdeApi(string nombre, decimal salarioBruto, string cedula)
        {
            string sexo = await _repo_empleado.ObtenerGeneroEmpleado(cedula);
            sexo = sexo.Equals("Masculino", StringComparison.OrdinalIgnoreCase) ? "male" : "female";

            return nombre switch
            {
                "poliza de vida" => await ObtenerDeduccionPolizaVida(sexo, cedula),
                "asociación solidarista" or "asociacion solidarista" => await ObtenerDeduccionAsociacion(salarioBruto, "Asociación"),
                "mediseguro" => await ObtenerDeduccionMediSeguro(sexo, cedula),
                _ => 0
            };
        }
        private async Task<decimal> ObtenerDeduccionPolizaVida(string sexo, string cedulaEmpleado)
        {
            try
            {
                var fechaNacimientoStr = await _repo_empleado.ObtenerFechaNacimientoEmpleado(cedulaEmpleado);
                var fechaNacimiento = DateTime.Parse(fechaNacimientoStr).ToString("yyyy-MM-dd");
                var url = $"https://poliza-friends-grg0h9g5crf2hwh8.southcentralus-01.azurewebsites.net/api/LifeInsurance" +
                          $"?date%20of%20birth={fechaNacimiento}&sex={sexo.ToLower()}";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("FRIENDS-API-TOKEN",
                    "1D0B194488C091852597B9AF7D1AA8F23D55C9784815489CF0A488B6F2C6D5C4C569AD51231FACB9920E5A763FE388247E03131D1601AC234E86BC0D266EB6A7");

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error {response.StatusCode}: {content}");
                    return 0;
                }

                using var jsonDoc = JsonDocument.Parse(content);
                if (jsonDoc.RootElement.TryGetProperty("monthlyCost", out var costo))
                {
                    return costo.GetDecimal();
                }
                return 0;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión o respuesta inválida: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return 0;
            }
        }

        private async Task<decimal> ObtenerDeduccionAsociacion(decimal salarioBruto, string nombreAsociacion)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = "https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate";
                var payload = new
                {
                    associationName = nombreAsociacion,
                    employeeSalary = salarioBruto
                };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Add("API-KEY", "Tralalerotralala");

                var response = await httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {responseContent}");
                    return 0;
                }

                using var doc = JsonDocument.Parse(responseContent);
                if (doc.RootElement.TryGetProperty("amountToCharge", out var amount))
                {
                    return amount.GetDecimal();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return 0;
            }
        }

        private async Task<decimal> ObtenerDeduccionMediSeguro(string sexo, string cedulaEmpleado)
        {
            using var httpClient = new HttpClient();

            var url = "https://mediseguro-vorlagenersteller-d4hmbvf7frg7aqan.southcentralus-01.azurewebsites.net/api/MediSeguroMonto";
            var fechaNacimientoStr = await _repo_empleado.ObtenerFechaNacimientoEmpleado(cedulaEmpleado);
            var fechaNacimiento = DateTime.Parse(fechaNacimientoStr).ToString("yyyy-MM-dd");
            var cantDependientes = await _repo_empleado.ObtenerCantDependientesEmpleado(cedulaEmpleado);
            var requestData = new
            {
                fechaNacimiento = fechaNacimiento,
                genero = sexo == "male"? "masculino": "femenino",
                cantidadDependientes = cantDependientes
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