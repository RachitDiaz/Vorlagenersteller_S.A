using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IBeneficiosRepository
    {
        string ObtenerCedulaJuridica(string correo);
        int ObtenerIdUsuario(string correo);
        List<BeneficioModel> ObtenerBeneficios(string cedulaEmpresa);
        int CrearBeneficio(BeneficioModel beneficio, string cedulaEmpresa, int IDUsuario);
        bool CrearRelacionEmpresaBeneficio(string cedulaEmpresa, int idBeneficio);
    }
}
