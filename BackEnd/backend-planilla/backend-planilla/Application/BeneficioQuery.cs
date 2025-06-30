using backend_planilla.Domain;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class BeneficioQuery : IBeneficioQuery
    {
        private readonly IBeneficioRepository _repo;

        public BeneficioQuery()
        {
            _repo = new BeneficioRepository();
        }

        public bool ActualizarBeneficiosEmpleado(string cedulaEmpleado, List<int> beneficios)
        {
            return _repo.ActualizarBeneficiosEmpleado(cedulaEmpleado, beneficios);
        }

        public List<BeneficioSimpleModel> ObtenerBeneficiosParaEmpleado(string correo)
        {
            return _repo.ObtenerBeneficiosParaEmpleado(correo);
        }

        public bool ActualizarDependientesEmpleado(string correo, int dependientes)
        {
            string cedula = _repo.ObtenerCedulaEmpleadoDesdeCorreo(correo);
            return _repo.ActualizarDependientesEmpleado( cedula, dependientes);
        }
        public List<BeneficioSimpleModel> ObtenerBeneficiosSeleccionadosPorEmpleado(string correo)
        {
            return _repo.ObtenerBeneficiosSeleccionadosPorEmpleado(correo);
        }

        public string ObtenerCedulaEmpleadoDesdeCorreo(string correo)
        {
            return _repo.ObtenerCedulaEmpleadoDesdeCorreo(correo);
        }
    }
}
