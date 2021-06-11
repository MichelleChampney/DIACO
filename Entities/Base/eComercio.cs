using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eComercio
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El NIT es obligatorio.")]
        [MaxLength(15)]
        public string NIT { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [MaxLength(100)]
        public string RazonSocial { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [MaxLength(9)]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato de correo electrónico no es correcto.")]
        [MaxLength(50)]
        public string CorreoElectronico { get; set; }
    }
}
