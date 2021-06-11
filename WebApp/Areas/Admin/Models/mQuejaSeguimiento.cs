using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class mQuejaSeguimiento
    {
        public eQuejaSeguimiento Obj { get; set; }
        public IEnumerable<SelectListItem> ListaEstado { get; set; }
        public mQuejaSeguimiento()
        {
            Obj = new eQuejaSeguimiento();
            ListaEstado = new List<SelectListItem>();
        }
    }
}
