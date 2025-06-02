using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace backend_planilla.Application
{
    public class BeneficiosQuery : IBeneficiosQuery
    {
        private readonly IBeneficiosRepository _beneficiosRepository;
        public BeneficiosQuery()
        {
            _beneficiosRepository = new BeneficiosRepository();
        }


        public bool FormatoCedulaValido(string correo)
        {
            return Regex.IsMatch(correo, @"^\d-\d{3}-\d{6}$");
        }
        public bool EsCorreoValido(string correo)
        {
            try
            {
                var mail = new MailAddress(correo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<BeneficioModel> GetBeneficios(string correo)
        {
            if (FormatoCedulaValido(correo))
                throw new FormatException("El formato de la cédula no es el correcto");

            var cedulaEmpresa = _beneficiosRepository.ObtenerCedulaJuridica(correo);
            if (string.IsNullOrEmpty(cedulaEmpresa))
                throw new KeyNotFoundException("No se encontró una cédula jurídica relacionada a este correo");

            var beneficios = _beneficiosRepository.ObtenerBeneficios(cedulaEmpresa);
            return beneficios;
        }

        public bool CrearBeneficio(BeneficioModel beneficio, string correo)
        {
            bool exito;
            if (!EsCorreoValido(correo))
                throw new FormatException("El formato del correo no es válido.");
            var cedulaEmpresa = _beneficiosRepository.ObtenerCedulaJuridica(correo);

            if (string.IsNullOrEmpty(cedulaEmpresa))
                throw new KeyNotFoundException("No se encontró una cédula jurídica relacionada a este correo");

            var IDUsuario = _beneficiosRepository.ObtenerIdUsuario(correo);
            if (IDUsuario == -1)
                throw new KeyNotFoundException("No se encontró una cédula de persona relacionada a este correo");
            try
            {
                var resultado = _beneficiosRepository.CrearBeneficio(beneficio, cedulaEmpresa, IDUsuario);
                if (resultado != -1)
                {
                    exito = _beneficiosRepository.CrearRelacionEmpresaBeneficio(cedulaEmpresa, resultado);
                }
                else
                {
                    throw new Exception("No se pudo ingresar el beneficio");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return exito;
        }
    }
}
