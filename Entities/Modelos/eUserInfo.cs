using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eUserInfo
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Password { get; set; }
    }
}
