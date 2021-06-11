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
    public class MunicipiosController : Controller
    {
        private readonly IGenericRepository _repo;

        public MunicipiosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index(int? idRegion, int? idDepartamento)
        {
            string valorRegion = (idRegion.HasValue) ? idRegion.ToString() : string.Empty;
            string valorDepartamento = (idDepartamento.HasValue) ? idDepartamento.ToString() : string.Empty;
            var listRegiones = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Regiones", string.Empty, false, valorRegion));
            if (idRegion.HasValue == false && listRegiones.ToList().Count > 0) idRegion = int.Parse(listRegiones.ToList()[0].Value);
            var listDepartamentos = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Departamentos", $"/{idRegion.GetValueOrDefault()}", false, valorDepartamento));
            ViewBag.ListaRegion = listRegiones;
            ViewBag.ListaDepartamento = listDepartamentos;
            ViewBag.Confirmacion = TempData["Confirmacion"];
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Details(string id)
        {
            string[] split = id.Split('_');
            var list = JsonConvert.DeserializeObject<IEnumerable<eCatalogo>>(_repo.GetAll("Municipios", $"/{split[0]}/{split[1]}", HttpContext.Session.GetString("Token")));
            return PartialView("_details", list);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create(int idRegion, int idDepartamento)
        {
            var objRegion = JsonConvert.DeserializeObject<eCatalogo>(_repo.Get(idRegion, "Regiones", HttpContext.Session.GetString("Token")));
            var objDepartamento = JsonConvert.DeserializeObject<eCatalogo>(_repo.Get(idDepartamento, "Departamentos", HttpContext.Session.GetString("Token")));
            return View(new eMunicipioVista() { IdDepartamento = idDepartamento, NombreDepartamento = objDepartamento.Nombre, IdRegion = idRegion, NombreRegion = objRegion.Nombre });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create([Bind("Nombre,IdDepartamento,NombreDepartamento,IdRegion,NombreRegion")] eMunicipioVista obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objMunicipio = new eMunicipio() { Nombre = obj.Nombre, IdDepartamento = obj.IdDepartamento };
                    _repo.Post(objMunicipio, "Municipios", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion, idDepartamento = obj.IdDepartamento });
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
            var obj = JsonConvert.DeserializeObject<eMunicipioVista>(_repo.Get(id, "Municipios", HttpContext.Session.GetString("Token")));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, [Bind("Id,Nombre,IdDepartamento,NombreDepartamento,IdRegion,NombreRegion")] eMunicipioVista obj)
        {
            if (id != obj.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var objMunicipio = new eMunicipio() { Id = obj.Id, Nombre = obj.Nombre, IdDepartamento = obj.IdDepartamento };
                    _repo.Put(id, objMunicipio, "Municipios", HttpContext.Session.GetString("Token"));
                    return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion, idDepartamento = obj.IdDepartamento });
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
            var obj = new eMunicipioVista();

            try
            {
                obj = JsonConvert.DeserializeObject<eMunicipioVista>(_repo.Get(id, "Municipios", HttpContext.Session.GetString("Token")));
                _repo.Delete(id, "Municipios", HttpContext.Session.GetString("Token"));
                TempData["Confirmacion"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index), new { idRegion = obj.IdRegion, idDepartamento = obj.IdDepartamento });
        }

        [AllowAnonymous]
        public JsonResult GetAllValueList(int idRegion, int idDepartamento, bool valorVacio)
        {
            var list = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Municipios", $"/{idRegion}/{idDepartamento}", valorVacio, string.Empty));
            return new JsonResult(list);
        }
    }
}
