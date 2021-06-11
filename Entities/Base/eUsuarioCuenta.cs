using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class eUsuarioCuenta
    {
        public string Usuario { get; set; }
        public Byte[] Salt { get; set; }
        public string NombreRol { get; set; }
    }
}
