using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class mQuejaEstado
    {
        public eQuejaEstado Obj { get; set; }
        public bool EnProceso { get; set; }
        public mQuejaEstado()
        {
            EnProceso = false;
            Obj = new eQuejaEstado();
        }
    }
}
