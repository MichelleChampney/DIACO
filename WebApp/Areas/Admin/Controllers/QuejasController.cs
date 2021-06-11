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
    public class QuejasController : Controller
    {
        private readonly IQuejaRepository _repo;

        public QuejasController(IQuejaRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Administrador,Supervisor,Consultor")]
        public IActionResult Index()
        {
            var ListaComercio = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Comercios", string.Empty, true, string.Empty));
            var ListaSucursal = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("ComercioSucursales", $"/0", true, string.Empty));
            var ListaRegion = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Regiones", string.Empty, true, string.Empty));
            var ListaDepartamento = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Departamentos", $"/0", true, string.Empty));
            var ListaMunicipio = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Municipios", $"/0/0", true, string.Empty));
            var ListaUbicacion = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Ubicaciones", $"/0/0/0", true, string.Empty));
            var ListaTipoQueja = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("QuejaTipos", string.Empty, true, string.Empty));

            ViewBag.ListaComercio = ListaComercio;
            ViewBag.ListaSucursal = ListaSucursal;
            ViewBag.ListaRegion = ListaRegion;
            ViewBag.ListaDepartamento = ListaDepartamento;
            ViewBag.ListaMunicipio = ListaMunicipio;
            ViewBag.ListaUbicacion = ListaUbicacion;
            ViewBag.ListaTipoQueja = ListaTipoQueja;

            return View();
        }

        [Authorize(Roles = "Administrador,Supervisor,Consultor")]
        public IActionResult GetListado(string id)
        {
            IEnumerable<eQuejaVista> list = new List<eQuejaVista>();

            try
            {
                string[] lId = id.Split('_');
                int? idComercio = null;
                int? idSucursal = null;
                int? idRegion = null;
                int? idDepartamento = null;
                int? idMunicipio = null;
                int? idUbicacion = null;
                int? idTipoQueja = null;

                if (lId[2] != "0") idComercio = int.Parse(lId[2]);
                if (lId[3] != "0") idSucursal = int.Parse(lId[3]);
                if (lId[4] != "0") idRegion = int.Parse(lId[4]);
                if (lId[5] != "0") idDepartamento = int.Parse(lId[5]);
                if (lId[6] != "0") idMunicipio = int.Parse(lId[6]);
                if (lId[7] != "0") idUbicacion = int.Parse(lId[7]);
                if (lId[8] != "0") idTipoQueja = int.Parse(lId[8]);

                list = JsonConvert.DeserializeObject<IEnumerable<eQuejaVista>>(_repo.GetConsulta("GetQuejasByFiltros", $"?FechaDel={lId[0]}&FechaAl={lId[1]}&IdComercio={idComercio}&IdSucursal={idSucursal}&IdRegion={idRegion}&IdDepartamento={idDepartamento}&IdMunicipio={idMunicipio}&IdUbicacion={idUbicacion}&IdTipo={idTipoQueja}&Estado={lId[9]}", HttpContext.Session.GetString("Token")));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return PartialView("_Listado", list);
        }

        [Authorize(Roles = "Administrador,Supervisor,Consultor")]
        public IActionResult IndexConteo()
        {
            var ListaComercio = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Comercios", string.Empty, true, string.Empty));
            var ListaSucursal = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("ComercioSucursales", $"/0", true, string.Empty));
            var ListaRegion = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Regiones", string.Empty, true, string.Empty));
            var ListaDepartamento = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Departamentos", $"/0", true, string.Empty));
            var ListaMunicipio = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Municipios", $"/0/0", true, string.Empty));
            var ListaUbicacion = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Ubicaciones", $"/0/0/0", true, string.Empty));
            var ListaTipoQueja = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("QuejaTipos", string.Empty, true, string.Empty));

            ViewBag.ListaComercio = ListaComercio;
            ViewBag.ListaSucursal = ListaSucursal;
            ViewBag.ListaRegion = ListaRegion;
            ViewBag.ListaDepartamento = ListaDepartamento;
            ViewBag.ListaMunicipio = ListaMunicipio;
            ViewBag.ListaUbicacion = ListaUbicacion;
            ViewBag.ListaTipoQueja = ListaTipoQueja;

            return View();
        }

        [Authorize(Roles = "Administrador,Supervisor,Consultor")]
        public IActionResult GetConteo(string id)
        {
            IEnumerable<eQuejaConteo> list = new List<eQuejaConteo>();

            try
            {
                string[] lId = id.Split('_');
                int? idComercio = null;
                int? idSucursal = null;
                int? idRegion = null;
                int? idDepartamento = null;
                int? idMunicipio = null;
                int? idUbicacion = null;
                int? idTipoQueja = null;
                bool? tipoConteo = null;

                if (lId[2] != "0") idComercio = int.Parse(lId[2]);
                if (lId[3] != "0") idSucursal = int.Parse(lId[3]);
                if (lId[4] != "0") idRegion = int.Parse(lId[4]);
                if (lId[5] != "0") idDepartamento = int.Parse(lId[5]);
                if (lId[6] != "0") idMunicipio = int.Parse(lId[6]);
                if (lId[7] != "0") idUbicacion = int.Parse(lId[7]);
                if (lId[8] != "0") idTipoQueja = int.Parse(lId[8]);
                if (lId[10] != "null") tipoConteo = bool.Parse(lId[10]);

                list = JsonConvert.DeserializeObject<IEnumerable<eQuejaConteo>>(_repo.GetConsulta("GetConteoQuejasByFiltros", $"?FechaDel={lId[0]}&FechaAl={lId[1]}&IdComercio={idComercio}&IdSucursal={idSucursal}&IdRegion={idRegion}&IdDepartamento={idDepartamento}&IdMunicipio={idMunicipio}&IdUbicacion={idUbicacion}&IdTipo={idTipoQueja}&Estado={lId[9]}&TipoConteo={tipoConteo}", HttpContext.Session.GetString("Token")));

                string data = string.Join(",", list.ToList().Where(b => b.Conteo > 0).Select(b => "{ \"categoria\": \"" + b.NombreComercio + " - " + b.NombreSucursal + "\", \"valor\": " + b.Conteo + " }").ToList());

                ViewBag.Data = data;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return PartialView("_Conteo", list);
        }

        [Authorize(Roles = "Administrador,Supervisor,Consultor")]
        public IActionResult IndexConteoVariado()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Supervisor,Consultor")]
        public IActionResult GetConteoVariado(string id)
        {
            string partialView = "";
            IEnumerable<eQuejaConteo> list = new List<eQuejaConteo>();

            try
            {
                string[] lId = id.Split('_');
                bool? tipoConteo = null;
                string action = "";
                string data = "";

                if (lId[1] != "null") tipoConteo = bool.Parse(lId[1]);

                switch (lId[2])
                {
                    case "Region":
                        action = "GetConteoQuejasPorRegion";
                        partialView = "_ConteoPorRegion";
                        break;
                    case "Departamento":
                        action = "GetConteoQuejasPorDepartamento";
                        partialView = "_ConteoPorDepartamento";
                        break;
                    case "Municipio":
                        action = "GetConteoQuejasPorMunicipio";
                        partialView = "_ConteoPorMunicipio";
                        break;
                    case "Comercio":
                        action = "GetConteoQuejasPorComercio";
                        partialView = "_ConteoPorComercio";
                        break;
                }

                list = JsonConvert.DeserializeObject<IEnumerable<eQuejaConteo>>(_repo.GetConsulta(action, $"/{lId[0]}/{tipoConteo}", HttpContext.Session.GetString("Token")));

                switch (lId[2])
                {
                    case "Region":
                        data = string.Join(",", list.ToList().Where(b => b.Conteo > 0).Select(b => "{ \"categoria\": \"" + b.NombreRegion + "\", \"valor\": " + b.Conteo + " }").ToList());
                        break;
                    case "Departamento":
                        data = string.Join(",", list.ToList().Where(b => b.Conteo > 0).Select(b => "{ \"categoria\": \"" + b.NombreDepartamento + "\", \"valor\": " + b.Conteo + " }").ToList());
                        break;
                    case "Municipio":
                        data = string.Join(",", list.ToList().Where(b => b.Conteo > 0).Select(b => "{ \"categoria\": \"" + b.NombreMunicipio + "\", \"valor\": " + b.Conteo + " }").ToList());
                        break;
                    case "Comercio":
                        data = string.Join(",", list.ToList().Where(b => b.Conteo > 0).Select(b => "{ \"categoria\": \"" + b.NombreComercio + "\", \"valor\": " + b.Conteo + " }").ToList());
                        break;
                }

                ViewBag.Data = data;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return PartialView(partialView, list);
        }

        [Authorize(Roles = "Administrador,Supervisor")]
        public IActionResult IndexPendientes(bool limpiar = false)
        {
            var obj = new eQuejaConsulta();

            if (limpiar)
                HttpContext.Session.SetString("QuejaConsulta", "");

            if (HttpContext.Session.GetString("QuejaConsulta") != "")
                obj = JsonConvert.DeserializeObject<eQuejaConsulta>(HttpContext.Session.GetString("QuejaConsulta"));
            else
            {
                obj.FechaDel = DateTime.Now.ToShortDateString();
                obj.FechaAl = DateTime.Now.ToShortDateString();
            }

            var ListaComercio = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("Comercios", string.Empty, true, string.Empty));
            var ListaSucursal = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("ComercioSucursales", $"/0", true, string.Empty));
            var ListaTipoQueja = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("QuejaTipos", string.Empty, true, string.Empty));

            ViewBag.ListaComercio = ListaComercio;
            ViewBag.ListaSucursal = ListaSucursal;
            ViewBag.ListaTipoQueja = ListaTipoQueja;

            return View(obj);
        }

        [Authorize(Roles = "Administrador,Supervisor")]
        public IActionResult GetListadoPendientes(string id)
        {
            IEnumerable<eQuejaVista> list = new List<eQuejaVista>();

            try
            {
                string[] lId = id.Split('_');
                int? idComercio = null;
                int? idSucursal = null;
                int? idTipoQueja = null;

                if (lId[2] != "0") idComercio = int.Parse(lId[2]);
                if (lId[3] != "0") idSucursal = int.Parse(lId[3]);
                if (lId[4] != "0") idTipoQueja = int.Parse(lId[4]);

                var obj = new eQuejaConsulta() { FechaDel = lId[0], FechaAl = lId[1], IdComercio = idComercio, IdSucursal = idSucursal, IdTipo = idTipoQueja };
                HttpContext.Session.SetString("QuejaConsulta", JsonConvert.SerializeObject(obj));

                list = JsonConvert.DeserializeObject<IEnumerable<eQuejaVista>>(_repo.GetConsulta("GetQuejasPendientes", $"?FechaDel={lId[0]}&FechaAl={lId[1]}&IdComercio={idComercio}&IdSucursal={idSucursal}&IdTipo={idTipoQueja}", HttpContext.Session.GetString("Token")));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return PartialView("_ListadoPendientes", list);
        }

        [Authorize(Roles = "Administrador,Supervisor")]
        public IActionResult CreateTracking(long idQueja)
        {
            var ObjQS = new mQuejaSeguimiento();
            ObjQS.Obj.IdQueja = idQueja;
            ObjQS.ListaEstado = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("QuejaEstados", string.Empty, false, string.Empty, "GetAllMovimientoValueList"));
            return View(ObjQS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Supervisor")]
        public IActionResult CreateTracking([Bind("Obj")] mQuejaSeguimiento ObjQS)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objSeguimiento = new eQuejaSeguimiento() { IdQueja = ObjQS.Obj.IdQueja, IdEstado = ObjQS.Obj.IdEstado, Comentario = ObjQS.Obj.Comentario };
                    _repo.PostSeguimiento(objSeguimiento, "Quejas", HttpContext.Session.GetString("Token"));
                    return RedirectToAction("IndexPendientes");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ObjQS.ListaEstado = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(_repo.GetAllValueList("QuejaEstados", string.Empty, false, string.Empty, "GetAllMovimientoValueList"));
            return View(ObjQS);
        }
    }
}
