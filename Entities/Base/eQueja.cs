using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eQueja
    {
        [Required(ErrorMessage = "El comercio es obligatorio.")]
        public int IdComercio { get; set; }
        [Required(ErrorMessage = "La sucursal es obligatoria.")]
        public int IdSucursal { get; set; }
        [Required(ErrorMessage = "El tipo de queja es obligatorio.")]
        public int IdTipoQueja { get; set; }
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(50)]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "La queja es obligatoria.")]
        [MaxLength(2000)]
        public string Queja { get; set; }
        [Required(ErrorMessage = "La petición es obligatoria.")]
        [MaxLength(2000)]
        public string Peticion { get; set; }
    }
}
