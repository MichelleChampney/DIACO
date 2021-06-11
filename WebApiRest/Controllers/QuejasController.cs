using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiRest.Repository;

namespace WebApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuejasController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public QuejasController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Guarda un comercio
        /// </summary>
        /// <param name="obj">Objeto con los datos del comercio</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] eQueja obj)
        {
            try
            {
                var result = await _repo.ExecuteScalarSPAsync("sp_SaveQuejas", new { @IdSucursal = obj.IdSucursal, @IdTipo = obj.IdTipoQueja, @Titulo = obj.Titulo, @Queja = obj.Queja, @Peticion = obj.Peticion });

                return Ok(result.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de una queja
        /// </summary>
        /// <param name="codigo">codigo de la queja</param>
        /// <returns>Objeto con los datos de la queja</returns>
        [HttpGet("{codigo}")]
        [AllowAnonymous]
        public async Task<ActionResult<eQuejaVistaCompleta>> Get(string codigo)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eQuejaVista>("sp_GetQuejas", new { @Codigo = codigo });
                var listSeguimiento = await _repo.GetAllSPAsync<eQuejaSeguimientoVista>("sp_GetQuejaSeguimientos", new { @IdQueja = obj.Id });

                if (obj == null)
                    return NotFound();
                else
                {
                    var ObjQVC = new eQuejaVistaCompleta();
                    ObjQVC.Obj = obj;
                    ObjQVC.ListObjSeguimiento = listSeguimiento;
                    return ObjQVC;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de de quejas pendientes de procesar
        /// </summary>
        /// <param name="obj">Objeto con los datos de los filtros de la consulta</param>
        /// <returns>Listado de quejas</returns>
        [HttpGet("GetQuejasPendientes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor")]
        public async Task<ActionResult<IEnumerable<eQuejaVista>>> GetQuejasPendientes([FromQuery] eQuejaConsulta obj)
        {
            try
            {
                var fechaAl = DateTime.Parse(obj.FechaAl);
                fechaAl = new DateTime(fechaAl.Year, fechaAl.Month, fechaAl.Day, 23, 59, 59);
                var list = await _repo.GetAllSPAsync<eQuejaVista>("sp_GetQuejasPendientes", new { @FechaDel = DateTime.Parse(obj.FechaDel).Date, @FechaAl = fechaAl, @IdComercio = obj.IdComercio, @IdSucursal = obj.IdSucursal, @IdTipo = obj.IdTipo });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Guarda un comercio
        /// </summary>
        /// <param name="obj">Objeto con los datos del comercio</param>
        /// <returns></returns>
        [HttpPost("PostSeguimiento")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor")]
        public async Task<ActionResult> PostSeguimiento([FromBody] eQuejaSeguimiento obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveQuejaSeguimientos", new { @IdQueja = obj.IdQueja, @IdEstado = obj.IdEstado, @Comentario = obj.Comentario });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado del conteo de quejas por sucursal
        /// </summary>
        /// <param name="obj">Objeto con los datos de los filtros de la consulta</param>
        /// <returns>Listado de conteo de quejas por sucursal</returns>
        [HttpGet("GetConteoQuejasByFiltros")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor,Consultor")]
        public async Task<ActionResult<IEnumerable<eQuejaConteo>>> GetConteoQuejasByFiltros([FromQuery] eQuejaConsulta obj)
        {
            try
            {
                var fechaAl = DateTime.Parse(obj.FechaAl);
                fechaAl = new DateTime(fechaAl.Year, fechaAl.Month, fechaAl.Day, 23, 59, 59);
                var list = await _repo.GetAllSPAsync<eQuejaConteo>("sp_GetConteoQuejasByFiltros", new { @FechaDel = DateTime.Parse(obj.FechaDel).Date, @FechaAl = fechaAl, @IdComercio = obj.IdComercio, @IdSucursal = obj.IdSucursal, @IdUbicacion = obj.IdUbicacion, @IdMunicipio = obj.IdMunicipio, @IdDepartamento = obj.IdDepartamento, @IdRegion = obj.IdRegion, @IdTipo = obj.IdTipo, @Estado = obj.Estado, @TipoConteo = obj.TipoConteo });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado del conteo de quejas por comercio
        /// </summary>
        /// <param name="anio">Anio en que se registraron las quejas</param>
        /// <param name="tipoConteo">Tipo de conteo</param>
        /// <returns>Listado de conteo de quejas por comercio</returns>
        [HttpGet("GetConteoQuejasPorComercio/{anio}/{tipoConteo?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor,Consultor")]
        public async Task<ActionResult<IEnumerable<eQuejaConteo>>> GetConteoQuejasPorComercio(int anio, bool? tipoConteo)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaConteo>("sp_GetConteoQuejasByComercio", new { @Anio = anio });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado del conteo de quejas por municipio
        /// </summary>
        /// <param name="anio">Anio en que se registraron las quejas</param>
        /// <param name="tipoConteo">Tipo de conteo</param>
        /// <returns>Listado de conteo de quejas por municipio</returns>
        [HttpGet("GetConteoQuejasPorMunicipio/{anio}/{tipoConteo?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor,Consultor")]
        public async Task<ActionResult<IEnumerable<eQuejaConteo>>> GetConteoQuejasPorMunicipio(int anio, bool? tipoConteo)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaConteo>("sp_GetConteoQuejasByMunicipio", new { @Anio = anio });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado del conteo de quejas por departamento
        /// </summary>
        /// <param name="anio">Anio en que se registraron las quejas</param>
        /// <param name="tipoConteo">Tipo de conteo</param>
        /// <returns>Listado de conteo de quejas por departamento</returns>
        [HttpGet("GetConteoQuejasPorDepartamento/{anio}/{tipoConteo?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor,Consultor")]
        public async Task<ActionResult<IEnumerable<eQuejaConteo>>> GetConteoQuejasPorDepartamento(int anio, bool? tipoConteo)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaConteo>("sp_GetConteoQuejasByDepartamento", new { @Anio = anio });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado del conteo de quejas por region
        /// </summary>
        /// <param name="anio">Anio en que se registraron las quejas</param>
        /// <param name="tipoConteo">Tipo de conteo</param>
        /// <returns>Listado de conteo de quejas por region</returns>
        [HttpGet("GetConteoQuejasPorRegion/{anio}/{tipoConteo?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor,Consultor")]
        public async Task<ActionResult<IEnumerable<eQuejaConteo>>> GetConteoQuejasPorRegion(int anio, bool? tipoConteo)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaConteo>("sp_GetConteoQuejasByRegion", new { @Anio = anio });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de de quejas
        /// </summary>
        /// <param name="obj">Objeto con los datos de los filtros de la consulta</param>
        /// <returns>Listado de quejas</returns>
        [HttpGet("GetQuejasByFiltros")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador,Supervisor,Consultor")]
        public async Task<ActionResult<IEnumerable<eQuejaVista>>> GetQuejasByFiltros([FromQuery] eQuejaConsulta obj)
        {
            try
            {
                var fechaAl = DateTime.Parse(obj.FechaAl);
                fechaAl = new DateTime(fechaAl.Year, fechaAl.Month, fechaAl.Day, 23, 59, 59);
                var list = await _repo.GetAllSPAsync<eQuejaVista>("sp_GetQuejasByFiltros", new { @FechaDel = DateTime.Parse(obj.FechaDel).Date, @FechaAl = fechaAl, @IdComercio = obj.IdComercio, @IdSucursal = obj.IdSucursal, @IdUbicacion = obj.IdUbicacion, @IdMunicipio = obj.IdMunicipio, @IdDepartamento = obj.IdDepartamento, @IdRegion = obj.IdRegion, @IdTipo = obj.IdTipo, @Estado = obj.Estado });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
