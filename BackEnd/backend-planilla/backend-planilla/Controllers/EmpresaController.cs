using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_planilla.Handlers;
using backend_planilla.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Reflection.Metadata;

namespace Empresa.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {

        private readonly EmpresaHandler _EmpresaHandler;
        public EmpresaController()
        {
            _EmpresaHandler = new EmpresaHandler();
        }

        [HttpPut("{cedula}")]
        public List<EmpresaModel> Get(string cedula)
        {
            //var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return _EmpresaHandler.ObtenerEmpresa(cedula);
        }

        [HttpGet]
        public List<EmpresaModel> Get()
        {
            var empresas = _EmpresaHandler.ObtenerEmpresas();
            return empresas;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CrearEmpresa(EmpresaModel empresa)
        {
            try
            {
                if (empresa == null)
                {
                    return BadRequest();
                }

                EmpresaHandler empresaHandler = new EmpresaHandler();
                var resultado = empresaHandler.CrearEmpresa(empresa);
                return new JsonResult(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando empresa");
            }
        }

       

    }
}
