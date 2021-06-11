using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class eUsuarioVista
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
    }
}
