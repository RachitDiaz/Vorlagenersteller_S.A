using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IBeneficiosRepository
    {
        string ObtenerCedulaJuridica(string correo);
        int ObtenerIdUsuario(string correo);
        List<BeneficioModel> ObtenerBeneficios(string cedulaEmpresa);
        int CrearBeneficio(BeneficioModel beneficio, string cedulaEmpresa, int idUsuario);
        bool CrearParametros(List<ParametroBeneficioModel> parametros, int idBeneficio);
        bool ExisteBeneficio(string cedulaEmpresa, string nombreBeneficio);
        int CopiarAPI(string nombreBeneficio, string cedulaEmpresa, int idUsuario);
        bool CopiarParametros(int idBeneficioNuevo, string nombreBeneficioCopiar);
        List<ParametroBeneficioModel> ObtenerParametrosBeneficio(int idBeneficio);
        bool SePuedeModificar(int idBeneficio, string cedulaEmpresa);
        bool ModificarBeneficio(BeneficioModel beneficioModificado, int idUsuario);
        bool ModificarParametros(List<ParametroBeneficioModel> parametros, int idBeneficio);
    }
}
