using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_planilla.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Reflection.Metadata;
using backend_planilla.Application;
using backend_planilla.Domain;

namespace backend_planilla.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {

        private readonly EmpresaHandler _EmpresaHandler;
        private readonly IEmpresaQuery _IEmpresaQuerry;
        public EmpresaController()
        {
            _EmpresaHandler = new EmpresaHandler();
            _IEmpresaQuerry = new EmpresaQuery();
        }

        [HttpPut("{cedula}")]
        public List<EmpresaModel> Get(string cedula)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return _EmpresaHandler.ObtenerEmpresa(cedula);
        }

        [HttpGet]
        public List<EmpresaModel> Get()
        {
            var empresas = _EmpresaHandler.ObtenerEmpresas();
            return empresas;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> RegistrarEmpresa(AgregarEmpresaModel infoEmpresa)
        {
            try
            {
                if (infoEmpresa == null)
                {
                    return BadRequest();
                }
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;


                IEmpresaQuery empresaQuery = new EmpresaQuery();
                var resultado = empresaQuery.RegistrarEmpresa(infoEmpresa, correo);

                if (resultado == false)
                {
                    return BadRequest();
                }

                return new JsonResult(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error registrando empresa");
            }
        }

    }
}
