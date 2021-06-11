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

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ComercioSucursalesController : Controller
    {
        private readonly IGenericRepository _repo;

        public ComercioSucursalesController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index(int idComercio)
        {
            var objComercio = JsonConvert.DeserializeObject<eComercio>(_repo.Get(idComercio, "Comercios", HttpContext.Session.GetString("Token")));
            ViewBag.IdComercio = objComercio.Id;
            ViewBag.Comercio = objComercio.Nombre;
            var list = JsonConvert.DeserializeObject<IEnumerable<eComercioSucursalVista>>(_repo.GetAll("ComercioSucursales", $"/{idComercio}", HttpContext.Session.GetString("Token")));
            ViewBag.Confirmacion = TempData["Confirmacion"];
            ViewBag.Error = TempData["Error"];
            return View(list);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create(int idComercio)
        {
            var objCS = new mComercioSucursal();
            var objComercio = JsonConvert.DeserializeObject<eComercio>(_repo.Get(idComercio, "Comercios", HttpContext.Session.GetString("Token")));
            objCS.Obj.IdComercio = idComercio;
            objCS.Obj.NombreComercio = objComercio.Nombre;
            this.LlenarListas(objCS);
            return View(objCS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create([Bind("Obj")] mComercioSucursal objCS)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objComercioSucursal = new eComercioSucursal() { IdComercio = objCS.Obj.IdComercio, Nombre = objCS.Obj.Nombre, IdUbicacion = objCS.Obj.IdUbicacion, Direccion = objCS.Obj.Direccion, Central = objCS.Obj.Central, Telefono = objCS.Obj.Telefono };
                    _repo.Post(objComercioSucursal, "ComercioSucursales", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idComercio = objCS.Obj.IdComercio });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            this.LlenarListas(objCS);
            return View(objCS);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var objCS = new mComercioSucursal();
            objCS.Obj = JsonConvert.DeserializeObject<eComercioSucursalVista>(_repo.Get(id, "ComercioSucursales", HttpContext.Session.GetString("Token")));
            if (objCS.Obj == null)
            {
                return NotFound();
            }
            this.LlenarListas(objCS);
            return View(objCS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Obj")] mComercioSucursal objCS)
        {
            if (id != objCS.Obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var objComercioSucursal = new eComercioSucursal() { Id = id, IdComercio = objCS.Obj.IdComercio, Nombre = objCS.Obj.Nombre, IdUbicacion = objCS.Obj.IdUbicacion, Direccion = objCS.Obj.Direccion, Central = objCS.Obj.Central, Telefono = objCS.Obj.Telefono };
                    _repo.Put(id, objComercioSucursal, "ComercioSucursales", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idComercio = objCS.Obj.IdComercio });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            this.LlenarListas(objCS);
            return View(objCS);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var objComercioSucursal = new eComercioSucursalVista();
            try
            {
                objComercioSucursal = JsonConvert.DeserializeObject<eComercioSucursalVista>(_repo.Get(id, "ComercioSucursales", HttpContext.Session.GetString("Token")));
                _repo.Delete(id, "ComercioSucursales", HttpContext.Session.GetString("Token"));
                TempData["Confirmacion"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index), new { idComercio = objComercioSucursal.IdComercio });
        }

        [AllowAnonymous]
        public JsonResult GetAllValueList(int idComercio, bool valorVacio)
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<eValueList>>(_repo.GetAllValueList("ComercioSucursales", $"/{idComercio}", valorVacio, string.Empty));
            return new JsonResult(list);
        }

        private void LlenarListas(mComercioSucursal objCS)
        {
            int idRegion = objCS.Obj.IdRegion;
            int idDepartamento = objCS.Obj.IdDepartamento;
            int idMunicipio = objCS.Obj.IdMunicipio;
            objCS.ListaRegion = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Regiones", string.Empty, false, string.Empty));
            if (idRegion == 0)
                idRegion = (objCS.ListaRegion.ToList().Count > 0) ? int.Parse(objCS.ListaRegion.ToList()[0].Value) : 0;
            objCS.ListaDepartamento = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Departamentos", $"/{idRegion}", false, string.Empty));
            if (idDepartamento == 0)
                idDepartamento = (objCS.ListaDepartamento.ToList().Count > 0) ? int.Parse(objCS.ListaDepartamento.ToList()[0].Value) : 0;
            objCS.ListaMunicipio = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Municipios", $"/{idRegion}/{idDepartamento}", false, string.Empty));
            if (idMunicipio == 0)
                idMunicipio = (objCS.ListaMunicipio.ToList().Count > 0) ? int.Parse(objCS.ListaMunicipio.ToList()[0].Value) : 0;
            objCS.ListaUbicacion = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Ubicaciones", $"/{idRegion}/{idDepartamento}/{idMunicipio}", false, string.Empty));
        }
    }
}
