using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IEmpresaRepository
    {
        bool RegistrarEmpresa(AgregarEmpresaModel empresa, string correo);
        public List<string> EliminarEmpresa(string cedulaEmpresa);
        public string ObtenerCedulaJuridica(string correo);
    }
}
