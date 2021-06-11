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
using WebApp.Repository;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartamentosController : Controller
    {
        private readonly IGenericRepository _repo;

        public DepartamentosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index(int? idRegion)
        {
            string valorRegion = (idRegion.HasValue) ? idRegion.ToString() : string.Empty;
            var listRegiones = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Regiones", string.Empty, false, valorRegion));
            ViewBag.ListaRegion = listRegiones;
            ViewBag.Confirmacion = TempData["Confirmacion"];
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Details(int id)
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eCatalogo>>(_repo.GetAll("Departamentos", $"/{id}", HttpContext.Session.GetString("Token")));
            return PartialView("_details", list);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create(int idRegion)
        {
            var objRegion = JsonConvert.DeserializeObject<eCatalogo>(_repo.Get(idRegion, "Regiones", HttpContext.Session.GetString("Token")));
            return View(new eDepartamentoVista() { IdRegion = idRegion, NombreRegion = objRegion.Nombre });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create([Bind("Nombre,IdRegion,NombreRegion")] eDepartamentoVista obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objDepartamento = new eDepartamento() { Nombre = obj.Nombre, IdRegion = obj.IdRegion };
                    _repo.Post(objDepartamento, "Departamentos", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(obj);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var obj = JsonConvert.DeserializeObject<eDepartamentoVista>(_repo.Get(id, "Departamentos", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Id,Nombre,IdRegion,NombreRegion")] eDepartamentoVista obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var objDepartamento = new eDepartamento() { Id = obj.Id, Nombre = obj.Nombre, IdRegion = obj.IdRegion };
                    _repo.Put(id, objDepartamento, "Departamentos", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(obj);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var obj = new eDepartamentoVista();

            try
            {
                obj = JsonConvert.DeserializeObject<eDepartamentoVista>(_repo.Get(id, "Departamentos", HttpContext.Session.GetString("Token")));
                _repo.Delete(id, "Departamentos", HttpContext.Session.GetString("Token"));
                TempData["Confirmacion"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion });
        }

        [AllowAnonymous]
        public JsonResult GetAllValueList(int idRegion, bool valorVacio)
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Departamentos", $"/{idRegion}", valorVacio, string.Empty));
            return Json(list); ;
        }
    }
}
