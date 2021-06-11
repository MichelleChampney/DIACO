using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eUsuarioPassword
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "La clave es obligatoria.")]
        [MaxLength(100)]
        public string Clave { get; set; }
        [Required(ErrorMessage = "La confirmación de clave es obligatoria.")]
        [MaxLength(100)]
        public string ConfirmacionClave { get; set; }
        [MaxLength(100)]
        public byte[] Salt { get; set; }
    }
}
