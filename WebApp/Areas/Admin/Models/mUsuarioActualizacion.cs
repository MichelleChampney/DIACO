using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class mUsuarioActualizacion
    {
        public eUsuarioActualizacion Obj { get; set; }
        public IEnumerable<SelectListItem> ListaRol { get; set; }
        public mUsuarioActualizacion()
        {
            Obj = new eUsuarioActualizacion();
            ListaRol = new List<SelectListItem>();
        }
    }
}
