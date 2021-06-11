using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Entities;
using WebApp.Repository;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuejaTiposController : Controller
    {
        private readonly IGenericRepository _repo;

        public QuejaTiposController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eQuejaTipo>>(_repo.GetAll("QuejaTipos", string.Empty, HttpContext.Session.GetString("Token")));
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
        public IActionResult Create([Bind("Abreviatura,Nombre")] eQuejaTipo obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Post(obj, "QuejaTipos", HttpContext.Session.GetString("Token"));
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
            eCatalogo obj = JsonConvert.DeserializeObject<eQuejaTipo>(_repo.Get(id, "QuejaTipos", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id, [Bind("Id,Abreviatura,Nombre")] eQuejaTipo obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Put(id, obj, "QuejaTipos", HttpContext.Session.GetString("Token"));
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
                _repo.Delete(id, "QuejaTipos", HttpContext.Session.GetString("Token"));
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
