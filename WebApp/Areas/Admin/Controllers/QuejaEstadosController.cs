using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Entities;
using WebApp.Repository;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Http;
using WebApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuejaEstadosController : Controller
    {
        private readonly IGenericRepository _repo;

        public QuejaEstadosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eQuejaEstado>>(_repo.GetAll("QuejaEstados", string.Empty, HttpContext.Session.GetString("Token")));
            ViewBag.Confirmacion = TempData["Confirmacion"];
            ViewBag.Error = TempData["Error"];
            return View(list);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View(new mQuejaEstado());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create([Bind("Obj")] mQuejaEstado objQE)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objEstado = new eQuejaEstado() { Nombre = objQE.Obj.Nombre, Inicial = objQE.Obj.Inicial, Final = objQE.Obj.Final, Rechazado = objQE.Obj.Rechazado };
                    _repo.Post(objEstado, "QuejaEstados", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(objQE);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var objQE = new mQuejaEstado();
            objQE.Obj = JsonConvert.DeserializeObject<eQuejaEstado>(_repo.Get(id, "QuejaEstados", HttpContext.Session.GetString("Token")));
            if (objQE.Obj == null)
            {
                return NotFound();
            }
            if (objQE.Obj.Inicial == false && objQE.Obj.Final == false || objQE.Obj.Rechazado == false)
                objQE.EnProceso = true;
            return View(objQE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Obj")] mQuejaEstado objQE)
        {
            if (id != objQE.Obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var objEstado = new eQuejaEstado() { Id = objQE.Obj.Id, Nombre = objQE.Obj.Nombre, Inicial = objQE.Obj.Inicial, Final = objQE.Obj.Final, Rechazado = objQE.Obj.Rechazado };
                    _repo.Put(id, objEstado, "QuejaEstados", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(objQE);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            try
            {
                _repo.Delete(id, "QuejaEstados", HttpContext.Session.GetString("Token"));
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
