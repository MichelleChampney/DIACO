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
    public class RolesController : Controller
    {
        private readonly IGenericRepository _repo;

        public RolesController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eCatalogo>>(_repo.GetAll("Roles", string.Empty, HttpContext.Session.GetString("Token")));
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
        public IActionResult Create([Bind("Nombre")] eCatalogo obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Post(obj, "Roles", HttpContext.Session.GetString("Token"));
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
            var obj = JsonConvert.DeserializeObject<eCatalogo>(_repo.Get(id, "Roles", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Id,Nombre")] eCatalogo obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Put(id, obj, "Roles", HttpContext.Session.GetString("Token"));
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
                _repo.Delete(id, "Roles", HttpContext.Session.GetString("Token"));
                TempData["Confirmacion"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
