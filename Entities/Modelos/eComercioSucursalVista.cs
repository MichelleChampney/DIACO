using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eComercioSucursalVista
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El comercio es obligatorio.")]
        public int IdComercio { get; set; }
        public string NombreComercio { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        public int IdRegion { get; set; }
        public string NombreRegion { get; set; }
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; }
        public int IdUbicacion { get; set; }
        public string NombreUbicacion { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [MaxLength(100)]
        public string Direccion { get; set; }
        public bool Central { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [MaxLength(9)]
        public string Telefono { get; set; }
    }
}
