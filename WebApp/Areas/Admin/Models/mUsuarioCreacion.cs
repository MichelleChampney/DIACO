using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class mUsuarioCreacion
    {
        public eUsuarioCreacion Obj { get; set; }
        public IEnumerable<SelectListItem> ListaRol { get; set; }
        public mUsuarioCreacion()
        {
            Obj = new eUsuarioCreacion();
            ListaRol = new List<SelectListItem>();
        }
    }
}
