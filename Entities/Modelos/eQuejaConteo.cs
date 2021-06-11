using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class eQuejaConteo
    {
        public string NombreComercio { get; set; }
        public string NombreSucursal { get; set; }
        public bool Central { get; set; }
        public string NombreUbicacion { get; set; }
        public string NombreMunicipio { get; set; }
        public string NombreDepartamento { get; set; }
        public string NombreRegion { get; set; }
        public int Conteo { get; set; }
    }
}
