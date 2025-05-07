using backend_planilla.Models;
using System.Collections.Generic;
using System.Linq;

namespace backend_planilla.Handlers
{
    public class EmpleadoHandler
    {
        private static List<EmpleadoModel> empleados = new List<EmpleadoModel>
        {
            new EmpleadoModel { Id = 1, Nombre = "Ana", Apellido1 = "Ramírez", Apellido2 = "Lopez", Cedula = "1-2345-6789", Posicion = "Gerente" },
            new EmpleadoModel { Id = 2, Nombre = "Carlos", Apellido1 = "Mora", Apellido2 = "Jiménez", Cedula = "2-1234-5678", Posicion = "Asistente" },
            new EmpleadoModel { Id = 3, Nombre = "Lucía", Apellido1 = "Gómez", Apellido2 = "Alvarado", Cedula = "3-9876-5432", Posicion = "Director" },
        };

        public List<EmpleadoModel> ObtenerEmpleados()
        {
            return empleados;
        }

        public void AgregarEmpleado(EmpleadoModel nuevo)
        {
            nuevo.Id = empleados.Max(e => e.Id) + 1;
            empleados.Add(nuevo);
        }

        public bool EliminarEmpleado(int id)
        {
            var emp = empleados.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                empleados.Remove(emp);
                return true;
            }
            return false;
        }

        public bool EditarEmpleado(EmpleadoModel actualizado)
        {
            var emp = empleados.FirstOrDefault(e => e.Id == actualizado.Id);
            if (emp != null)
            {
                emp.Nombre = actualizado.Nombre;
                emp.Apellido1 = actualizado.Apellido1;
                emp.Apellido2 = actualizado.Apellido2;
                emp.Cedula = actualizado.Cedula;
                emp.Posicion = actualizado.Posicion;
                return true;
            }
            return false;
        }
    }
}
