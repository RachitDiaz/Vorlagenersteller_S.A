﻿using backend_planilla.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace backend_planilla.Models
{
    public class BeneficioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string? ServicioExterno { get; set; }
        public int MesesMinimos { get; set; }
        public int CantidadParametros { get; set; }
        public List<ParametroBeneficioModel> Parametros { get; set; }
    }
}
