using backend_planilla.Domain;
using backend_planilla.Exceptions;
using backend_planilla.Infraestructure;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.Json;
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
        public BeneficiosQuery(IBeneficiosRepository beneficiosRepository)
        {
            _beneficiosRepository = beneficiosRepository;
        }

        private bool EsCorreoValido(string correo)
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
            if (!EsCorreoValido(correo))
                throw new FormatException("El formato del correo no es válido");

            var cedulaEmpresa = _beneficiosRepository.ObtenerCedulaJuridica(correo);
            Console.WriteLine(cedulaEmpresa);
            if (string.IsNullOrEmpty(cedulaEmpresa))
                throw new KeyNotFoundException("No se encontró una cédula jurídica relacionada a este correo");
            List<BeneficioModel> beneficios;
            try
            {
                beneficios = _beneficiosRepository.ObtenerBeneficios(cedulaEmpresa);
            }
            catch (Exception ex)
            {
                throw;
            }
            return beneficios;
        }

        public bool CrearBeneficio(BeneficioModel beneficio, string correo)
        {
            bool exito = false;
            if (!EsCorreoValido(correo))
                throw new FormatException("El formato del correo no es válido.");
            var cedulaEmpresa = _beneficiosRepository.ObtenerCedulaJuridica(correo);

            if (string.IsNullOrEmpty(cedulaEmpresa))
                throw new KeyNotFoundException("No se encontró una cédula jurídica relacionada a este correo");

            var idUsuario = _beneficiosRepository.ObtenerIdUsuario(correo);
            if (idUsuario == -1)
                throw new KeyNotFoundException("No se encontró una cédula de persona relacionada a este correo");
            try
            {
                if (_beneficiosRepository.ExisteBeneficio(cedulaEmpresa, beneficio.Nombre))
                    throw new ResourceAlreadyExistsException("La empresa ya tiene este beneficio registrado");
                if (beneficio.Tipo == "API")
                {
                    exito = CopiarBeneficioAPI(beneficio.Nombre, cedulaEmpresa, idUsuario);
                } else
                {
                    var resultado = _beneficiosRepository.CrearBeneficio(beneficio, cedulaEmpresa, idUsuario);
                    if (resultado != -1)
                    {
                        exito = _beneficiosRepository.CrearParametros(beneficio.Parametros, resultado);
                    } else
                    {
                        throw new Exception("No se pudo ingresar el beneficio");
                    }
                }
                    
            }
            catch (Exception ex)
            {
                throw;
            }
            return exito;
        }

        public bool CopiarBeneficioAPI(string nombreBeneficio, string cedulaEmpresa, int idUsuario)
        {
            bool exito = false;
            try
            {
                var idBeneficioNuevo = _beneficiosRepository.CopiarAPI(nombreBeneficio, cedulaEmpresa, idUsuario);
                exito = _beneficiosRepository.CopiarParametros(idBeneficioNuevo, nombreBeneficio);
            }
            catch(Exception ex) { throw; }
            return exito;

        }

        public bool ModificarBeneficio(BeneficioModel beneficioModificado,
            BeneficioModel beneficioOriginal, string correo)
        {
            bool exito = false;
            if (!EsCorreoValido(correo))
                throw new FormatException("El formato del correo no es válido.");
            var cedulaEmpresa = _beneficiosRepository.ObtenerCedulaJuridica(correo);

            if (string.IsNullOrEmpty(cedulaEmpresa))
                throw new KeyNotFoundException("No se encontró una cédula jurídica relacionada a este correo");

            var idUsuario = _beneficiosRepository.ObtenerIdUsuario(correo);
            if (idUsuario == -1)
                throw new KeyNotFoundException("No se encontró una cédula de persona relacionada a este correo");

            beneficioModificado.Id = beneficioOriginal.Id;
            if (!HayCambiosBeneficios(beneficioModificado, beneficioOriginal))
            {
                throw new InvalidOperationException("No hay cambios que realizar en los beneficios");
            }

            if (!SePuedeModificarBeneficio(beneficioModificado.Id, cedulaEmpresa))
            {
                throw new InvalidOperationException("El beneficio no se puede modificar porque ya fué seleccionado por un empleado");
            }

            exito = _beneficiosRepository.ModificarBeneficio(beneficioModificado, idUsuario);
            if (exito)
            {
                exito = _beneficiosRepository.ModificarParametros(beneficioModificado.Parametros, beneficioModificado.Id);
            }
            return exito;
        }

        private bool SePuedeModificarBeneficio(int Id, string cedulaEmpresa)
        {
            return _beneficiosRepository.SePuedeModificar(Id, cedulaEmpresa);
        }
        private bool HayCambiosBeneficios(BeneficioModel modificado, BeneficioModel original)
        {
            if (original.Nombre != modificado.Nombre ||
                original.Descripcion != modificado.Descripcion ||
                original.Tipo != modificado.Tipo ||
                original.MesesMinimos != modificado.MesesMinimos ||
                original.CantidadParametros != modificado.CantidadParametros)
                return true;

            if (original.Parametros.Count != modificado.Parametros.Count)
                return true;

            for (int i = 0; i < original.Parametros.Count; i++)
            {
                var p1 = original.Parametros[i];
                var p2 = modificado.Parametros[i];

                if (p1.Nombre != p2.Nombre ||
                    p1.TipoDeDatoParametro != p2.TipoDeDatoParametro ||
                    p1.TipoValorParametro != p2.TipoValorParametro ||
                    p1.ValorDelParametro != p2.ValorDelParametro)
                    return true;
            }

            return false;
        }
    }
}
