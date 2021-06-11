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
    public class UbicacionesController : Controller
    {
        private readonly IGenericRepository _repo;

        public UbicacionesController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index(int? idRegion, int? idDepartamento, int? idMunicipio)
        {
            string valorRegion = (idRegion.HasValue) ? idRegion.ToString() : string.Empty;
            string valorDepartamento = (idDepartamento.HasValue) ? idDepartamento.ToString() : string.Empty;
            string valorMunicipio = (idMunicipio.HasValue) ? idMunicipio.ToString() : string.Empty;
            var listRegiones = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Regiones", string.Empty, false, valorRegion));
            if (idRegion.HasValue == false && listRegiones.ToList().Count > 0) idRegion = int.Parse(listRegiones.ToList()[0].Value);
            var listDepartamentos = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Departamentos", $"/{idRegion.GetValueOrDefault()}", false, valorDepartamento));
            if (idDepartamento.HasValue == false && listDepartamentos.ToList().Count > 0) idDepartamento = int.Parse(listDepartamentos.ToList()[0].Value);
            var listMunicipios = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Municipios", $"/{idRegion.GetValueOrDefault()}/{idDepartamento.GetValueOrDefault()}", false, valorMunicipio));
            ViewBag.ListaRegion = listRegiones;
            ViewBag.ListaDepartamento = listDepartamentos;
            ViewBag.ListaMunicipio = listMunicipios;
            ViewBag.Confirmacion = TempData["Confirmacion"];
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Details(string id)
        {
            string[] split = id.Split('_');
            var list = JsonConvert.DeserializeObject<IEnumerable<eCatalogo>>(_repo.GetAll("Ubicaciones", $"/{split[0]}/{split[1]}/{split[2]}", HttpContext.Session.GetString("Token")));
            return PartialView("_details", list);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create(int idRegion, int idDepartamento, int idMunicipio)
        {
            var objRegion = JsonConvert.DeserializeObject<eCatalogo>(_repo.Get(idRegion, "Regiones", HttpContext.Session.GetString("Token")));
            var objDepartamento = JsonConvert.DeserializeObject<eCatalogo>(_repo.Get(idDepartamento, "Departamentos", HttpContext.Session.GetString("Token")));
            var objMunicipio = JsonConvert.DeserializeObject<eCatalogo>(_repo.Get(idMunicipio, "Municipios", HttpContext.Session.GetString("Token")));
            return View(new eUbicacionVista() { IdMunicipio = idMunicipio, NombreMunicipio = objMunicipio.Nombre, IdDepartamento = idDepartamento, NombreDepartamento = objDepartamento.Nombre, IdRegion = idRegion, NombreRegion = objRegion.Nombre });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create([Bind("Nombre,IdMunicipio,NombreMunicipio,IdDepartamento,NombreDepartamento,IdRegion,NombreRegion")] eUbicacionVista obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objUbicacion = new eUbicacion() { Nombre = obj.Nombre, IdMunicipio = obj.IdMunicipio };
                    _repo.Post(objUbicacion, "Ubicaciones", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion, idDepartamento = obj.IdDepartamento, idMunicipio = obj.IdMunicipio });
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
            var obj = JsonConvert.DeserializeObject<eUbicacionVista>(_repo.Get(id, "Ubicaciones", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Id,Nombre,IdMunicipio,NombreMunicipio,IdDepartamento,NombreDepartamento,IdRegion,NombreRegion")] eUbicacionVista obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var objUbicacion = new eUbicacion() { Id = obj.Id, Nombre = obj.Nombre, IdMunicipio = obj.IdMunicipio };
                    _repo.Put(id, objUbicacion, "Ubicaciones", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion, idDepartamento = obj.IdDepartamento, idMunicipio = obj.IdMunicipio });
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
            var obj = new eUbicacionVista();

            try
            {
                obj = JsonConvert.DeserializeObject<eUbicacionVista>(_repo.Get(id, "Ubicaciones", HttpContext.Session.GetString("Token")));
                _repo.Delete(id, "Ubicaciones", HttpContext.Session.GetString("Token"));
                TempData["Confirmacion"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion, idDepartamento = obj.IdDepartamento, idMunicipio = obj.IdMunicipio });
        }

        [AllowAnonymous]
        public JsonResult GetAllValueList(int idRegion, int idDepartamento, int idMunicipio, bool valorVacio)
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Ubicaciones", $"/{idRegion}/{idDepartamento}/{idMunicipio}", valorVacio, string.Empty));
            return new JsonResult(list);
        }
    }
}
