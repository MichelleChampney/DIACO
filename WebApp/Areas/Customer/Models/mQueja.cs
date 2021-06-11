using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Customer.Models
{
    public class mQueja
    {
        public eQueja Obj { get; set; }
        public IEnumerable<SelectListItem> ListaTipoQueja { get; set; }
        public IEnumerable<SelectListItem> ListaComercio { get; set; }
        public IEnumerable<SelectListItem> ListaSucursal { get; set; }
        public mQueja()
        {
            Obj = new eQueja();
            ListaTipoQueja = new List<SelectListItem>();
            ListaComercio = new List<SelectListItem>();
            ListaSucursal = new List<SelectListItem>();
        }
    }
}
