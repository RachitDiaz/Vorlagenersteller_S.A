﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public interface IEmpresaQuery
    {
        bool RegistrarEmpresa(AgregarEmpresaModel empresa, string correo);
        bool EliminarEmpresa(string correo);
    }
}
