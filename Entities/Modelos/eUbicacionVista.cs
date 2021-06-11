using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eUbicacionVista
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(30)]
        public string Nombre { get; set; }
        public int IdRegion { get; set; }
        public string NombreRegion { get; set; }
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        [Required(ErrorMessage = "El municipio es obligatorio.")]
        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; }
    }
}
