using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eQuejaSeguimiento
    {
        [Required(ErrorMessage = "La queja es obligatoria.")]
        public long IdQueja { get; set; }
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public int IdEstado { get; set; }
        [Required(ErrorMessage = "El comentario es obligatorio.")]
        [MaxLength(1000)]
        public string Comentario { get; set; }
    }
}
