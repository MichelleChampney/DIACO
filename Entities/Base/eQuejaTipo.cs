using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eQuejaTipo : eCatalogo
    {
        [Required(ErrorMessage = "La abreviatura es obligatoria.")]
        [MaxLength(3)]
        public string Abreviatura { get; set; }
    }
}
