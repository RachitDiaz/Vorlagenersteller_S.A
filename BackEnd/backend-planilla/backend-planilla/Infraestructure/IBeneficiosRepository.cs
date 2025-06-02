using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IBeneficiosRepository
    {
        string ObtenerCedulaJuridica(string correo);
        int ObtenerIdUsuario(string correo);
        List<BeneficioModel> ObtenerBeneficios(string cedulaEmpresa);
        int CrearBeneficio(BeneficioModel beneficio, string cedulaEmpresa, int IDUsuario);
        bool CrearParametros(List<ParametroBeneficioModel> parametros, int IdBeneficio);
        bool ExisteBeneficio(string cedulaEmpresa, string nombreBeneficio);
        int CopiarAPI(string nombreBeneficio, string cedulaEmpresa, int idUsuario);
        bool CopiarParametros(int idBeneficioNuevo, string nombreBeneficioCopiar);
    }
}
