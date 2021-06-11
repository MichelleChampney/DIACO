using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Models
{
    public class mUsuarioPassword
    {
        public string Usuario { get; set; }
        public eUsuarioPassword Obj { get; set; }
        public mUsuarioPassword()
        {
            Usuario = string.Empty;
            Obj = new eUsuarioPassword();
        }
    }
}
