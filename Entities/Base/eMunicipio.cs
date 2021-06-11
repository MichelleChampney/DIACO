using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eMunicipio
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(30)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El departamento es obligatorio.")]
        public int IdDepartamento { get; set; }
    }
}
