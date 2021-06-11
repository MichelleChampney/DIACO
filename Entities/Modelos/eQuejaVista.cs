using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class eQuejaVista
    {
        public long Id { get; set; }
        public string Codigo { get; set; }
        public string NombreTipoQueja { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Queja { get; set; }
        public string Peticion { get; set; }
        public string NombreEstadoQueja { get; set; }
        public string NombreComercio { get; set; }
        public string NombreSucursal { get; set; }
        public bool Central { get; set; }
        public string NombreUbicacion { get; set; }
        public string NombreMunicipio { get; set; }
        public string NombreDepartamento { get; set; }
        public string NombreRegion { get; set; }
    }
}
