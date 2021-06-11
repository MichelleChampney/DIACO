using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class eQuejaVistaCompleta
    {
        public eQuejaVista Obj { get; set; }
        public IEnumerable<eQuejaSeguimientoVista> ListObjSeguimiento { get; set; }
        public eQuejaVistaCompleta()
        {
            Obj = new eQuejaVista();
            ListObjSeguimiento = new List<eQuejaSeguimientoVista>();
        }
    }
}
