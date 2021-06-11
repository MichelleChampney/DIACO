using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.Models
{
    public class mComercioSucursal
    {
        public eComercioSucursalVista Obj { get; set; }
        public IEnumerable<SelectListItem> ListaRegion { get; set; }
        public IEnumerable<SelectListItem> ListaDepartamento { get; set; }
        public IEnumerable<SelectListItem> ListaMunicipio { get; set; }
        public IEnumerable<SelectListItem> ListaUbicacion { get; set; }
        public mComercioSucursal()
        {
            Obj = new eComercioSucursalVista();
            ListaRegion= new List<SelectListItem>();
            ListaDepartamento = new List<SelectListItem>();
            ListaMunicipio = new List<SelectListItem>();
            ListaUbicacion = new List<SelectListItem>();
        }
    }
}
