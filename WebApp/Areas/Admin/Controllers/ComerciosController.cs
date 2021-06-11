using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Repository;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ComerciosController : Controller
    {
        private readonly IGenericRepository _repo;

        public ComerciosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eComercio>>(_repo.GetAll("Comercios", string.Empty, HttpContext.Session.GetString("Token")));
            ViewBag.Confirmacion = TempData["Confirmacion"];
            ViewBag.Error = TempData["Error"];
            return View(list);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create([Bind("NIT,Nombre,RazonSocial,Telefono,CorreoElectronico")] eComercio obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Post(obj, "Comercios", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index));
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
            var obj = JsonConvert.DeserializeObject<eComercio>(_repo.Get(id, "Comercios", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Id,NIT,Nombre,RazonSocial,Telefono,CorreoElectronico")] eComercio obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Put(id, obj, "Comercios", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index));
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
            try
            {
                _repo.Delete(id, "Comercios", HttpContext.Session.GetString("Token"));
                TempData["Confirmacion"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public JsonResult GetAllValueList(bool valorVacio)
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eValueList>>(_repo.GetAllValueList("Comercios", string.Empty, valorVacio, string.Empty));
            return new JsonResult(list);
        }
    }
}
