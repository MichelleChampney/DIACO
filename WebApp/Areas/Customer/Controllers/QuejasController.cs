using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Customer.Models;
using WebApp.Repository;

namespace WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [AllowAnonymous]
    public class QuejasController : Controller
    {
        private readonly IQuejaRepository _repo;

        public QuejasController(IQuejaRepository repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            var objQC = new mQueja();
            this.LlenarListas(objQC);
            return View(objQC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Obj")] mQueja objQC)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var codigo = _repo.PostScalar(objQC.Obj, "Quejas", string.Empty);
                    TempData["Mensaje"] = $"Se generó la queja {JsonConvert.DeserializeObject<string>(codigo)}";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            this.LlenarListas(objQC);
            return View(objQC);
        }

        private void LlenarListas(mQueja objQC)
        {
            objQC.ListaTipoQueja = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("QuejaTipos", string.Empty, false, string.Empty));
            objQC.ListaComercio = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Comercios", string.Empty, false, string.Empty));
            int idComercio = (objQC.ListaComercio.ToList().Count > 0) ? int.Parse(objQC.ListaComercio.ToList()[0].Value) : 0;
            objQC.ListaSucursal = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("ComercioSucursales", $"/{idComercio}", false, string.Empty));
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult GetDetails(string id)
        {
            var obj = JsonConvert.DeserializeObject<eQuejaVistaCompleta>(_repo.GetQueja(id, "Quejas", string.Empty));
            if (obj == null)
            {
                return NotFound();
            }
            return PartialView("_Detalle", obj);
        }
    }
}
