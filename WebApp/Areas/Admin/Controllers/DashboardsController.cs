using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador,Supervisor,Consultor")]

    public class DashboardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
