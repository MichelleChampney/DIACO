using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eUsuarioCreacion
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato de usuario no es correcto.")]
        [MaxLength(50)]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La clave es obligatoria.")]
        [MaxLength(50)]
        public string Clave { get; set; }
        [Required(ErrorMessage = "La confirmación de clave es obligatoria.")]
        [MaxLength(100)]
        public string ConfirmacionClave { get; set; }
        [MaxLength(100)]
        public byte[] Salt { get; set; }
        public bool Activo { get; set; }
        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int IdRol { get; set; }
    }
}
