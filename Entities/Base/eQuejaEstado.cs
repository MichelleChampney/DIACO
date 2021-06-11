using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class eQuejaEstado
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(30)]
        public string Nombre { get; set; }
        public bool Inicial { get; set; }
        public bool Final { get; set; }
        public bool Rechazado { get; set; }
    }
}
