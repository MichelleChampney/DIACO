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
using WebApp.Areas.Admin.Models;
using WebApp.Repository;
using WebApp.Services;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly HashService _hashService;
        public UsuariosController(IUserRepository repo, HashService hashService)
        {
            _repo = repo;
            _hashService = hashService;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eUsuarioVista>>(_repo.GetAll("Usuarios", string.Empty, HttpContext.Session.GetString("Token")));
            ViewBag.Confirmacion = TempData["Confirmacion"];
            ViewBag.Error = TempData["Error"];
            return View(list);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            var objUC = new mUsuarioCreacion();
            objUC.ListaRol = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Roles", string.Empty, false, string.Empty));
            return View(objUC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create([Bind("Obj")] mUsuarioCreacion objUC)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidarClave(objUC.Obj.Clave, objUC.Obj.ConfirmacionClave);
                    var clave = _hashService.Hash(objUC.Obj.Clave);
                    var confirmacionClave = _hashService.Hash(objUC.Obj.ConfirmacionClave, clave.Salt);
                    var objUsuarioCreacion = new eUsuarioCreacion() { Nombre = objUC.Obj.Nombre, Usuario = objUC.Obj.Usuario, Clave = clave.Hash, ConfirmacionClave = confirmacionClave.Hash, Salt = clave.Salt, Activo = objUC.Obj.Activo, IdRol = objUC.Obj.IdRol };
                    var length = System.Text.ASCIIEncoding.ASCII.GetByteCount(objUsuarioCreacion.Salt.ToString());
                    _repo.Post(objUsuarioCreacion, "Usuarios", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            objUC.ListaRol = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Roles", string.Empty, false, string.Empty));
            return View(objUC);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var obj = JsonConvert.DeserializeObject<eUsuarioVista>(_repo.Get(id, "Usuarios", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }

            var objUA = new mUsuarioActualizacion();
            objUA.Obj = new eUsuarioActualizacion() { Id = obj.Id, Nombre = obj.Nombre, Activo = obj.Activo, IdRol = obj.IdRol };
            objUA.ListaRol = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Roles", string.Empty, false, string.Empty));
            return View(objUA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Obj")] mUsuarioActualizacion objUA)
        {
            if (id != objUA.Obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var objUsuarioActualizacion = new eUsuarioActualizacion() { Id = objUA.Obj.Id, Nombre = objUA.Obj.Nombre, Activo = objUA.Obj.Activo, IdRol = objUA.Obj.IdRol };
                    _repo.Put(id, objUsuarioActualizacion, "Usuarios", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            objUA.ListaRol = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Roles", string.Empty, false, string.Empty));
            return View(objUA);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult EditPassword(int id)
        {
            var obj = JsonConvert.DeserializeObject<eUsuarioVista>(_repo.Get(id, "Usuarios", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }

            var objUP = new mUsuarioPassword() { Usuario = obj.Usuario };
            objUP.Obj.Id = id;
            return View(objUP);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult EditPassword(int id, [Bind("Usuario,Obj")] mUsuarioPassword objUP)
        {
            if (id != objUP.Obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    ValidarClave(objUP.Obj.Clave, objUP.Obj.ConfirmacionClave);
                    var clave = _hashService.Hash(objUP.Obj.Clave);
                    var confirmacionClave = _hashService.Hash(objUP.Obj.ConfirmacionClave, clave.Salt);
                    var objUsuarioPassword = new eUsuarioPassword() { Id = objUP.Obj.Id, Clave = clave.Hash, ConfirmacionClave = confirmacionClave.Hash, Salt = clave.Salt };
                    _repo.PutPassword(id, objUsuarioPassword, "Usuarios", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(objUP);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            try
            {
                _repo.Delete(id, "Usuarios", HttpContext.Session.GetString("Token"));
                TempData["Confirmacion"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private static void ValidarClave(string clave, string confirmacionClave)
        {
            if (clave.Length < 8)
                throw new Exception("la clave debe tener una longitud mínima de 8 caracterres.");
            if (clave.Any(char.IsUpper) == false)
                throw new Exception("La clave debe contener mayúsculas.");
            if (clave.Any(char.IsLower) == false)
                throw new Exception("La contraseña debe contener minúsculas.");
            if (clave.Any(char.IsDigit) == false)
                throw new Exception("La contraseña debe contener números.");
            if (clave.Any(char.IsSymbol) == false)
                throw new Exception("La contraseña debe contener símbolos.");
            if (clave != confirmacionClave)
                throw new Exception("Las claves no coinciden.");
        }
    }
}
