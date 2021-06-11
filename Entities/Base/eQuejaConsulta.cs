using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eQuejaConsulta
    {
        [Required]
        public string FechaDel { get; set; }
        [Required]
        public string FechaAl { get; set; }
        public int? IdComercio { get; set; }
        public int? IdSucursal { get; set; }
        public int? IdRegion  { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdMunicipio { get; set; }
        public int? IdUbicacion { get; set; }
        public int? IdTipo { get; set; }
        [Required]
        [Range(0, 4)]
        public int Estado { get; set; }
        public bool? TipoConteo { get; set; }
    }
}
