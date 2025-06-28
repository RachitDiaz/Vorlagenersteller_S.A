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


        public async Task<List<DeduccionCalculada>> CalcularDeduccioensBeneficios(string correo)
        {
            var cedulaEmpleado = _repo_beneficio.ObtenerCedulaEmpleadoDesdeCorreo(correo);
            var salarioBruto = await _repo_empleado.ObtenerSalarioBruto(cedulaEmpleado);
            var beneficios = await _repo_empleado.ObtenerBeneficiosEmpleado(cedulaEmpleado);
            List<DeduccionCalculada> resultado = new List<DeduccionCalculada>();
            foreach (DeduccionBeneficioModel beneficio in beneficios)
            {
                decimal deduccion = 0;
                const decimal cienPorciento = 100m;
                const decimal cero = 0;
                var parametros = _repo_beneficios.ObtenerParametrosBeneficio(beneficio.IDBeneficio);

                if (beneficio.Tipo.ToLower() == "porcentaje")
                {
                    foreach (var param in parametros)
                    {
                        if (param.TipoValorParametro.ToLower() == "porcentaje")
                        {
                            deduccion += salarioBruto * (param.ValorDelParametro / cienPorciento);
                        }
                    }

                }
                else if (beneficio.Tipo.ToLower() == "fijo" || beneficio.Tipo.ToLower() == "montofijo")
                {
                    if (parametros.Count() >= cero)
                    {
                        foreach (var param in parametros)
                        {
                            if (param.TipoValorParametro.ToLower() == "fijo")
                            {
                                deduccion += param.ValorDelParametro;
                            }
                        }
                    }
                    else
                    {
                        deduccion = 0;
                    }
                }
                else if (beneficio.Tipo.ToLower() == "api")
                {
                    string nombre = beneficio.Nombre.ToLower();
                    if (nombre == "poliza de vida")
                    {
                        var sexo = await _repo_empleado.ObtenerGeneroEmpleado(cedulaEmpleado);
                        sexo = sexo.Equals("Masculino") ? "male" : "female";
                        deduccion = await ObtenerDeduccionPolizaVida(sexo, cedulaEmpleado);
                    }
                    else if (nombre == "asociación solidarista" || nombre == "asociacion solidarista")
                    {
                        deduccion = await ObtenerDeduccionAsociacion(salarioBruto, "Asociación");
                    }
                    else if (nombre == "mediseguro")
                    {

                        var sexo = await _repo_empleado.ObtenerGeneroEmpleado(cedulaEmpleado);
                        deduccion = await ObtenerDeduccionMediSeguro(sexo, cedulaEmpleado);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                deduccion = Math.Min(deduccion, salarioBruto);


                if (deduccion < 0)
                {
                    deduccion = 0;

                }
                else
                {
                    resultado.Add(new DeduccionCalculada
                    {
                        NombreBeneficio = beneficio.Nombre,
                        MontoReducido = Math.Round(deduccion, 2)
                    });
                }

            }

            // Siempre retornar una lista de 3 elementos (relleno con ceros si hay menos)
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