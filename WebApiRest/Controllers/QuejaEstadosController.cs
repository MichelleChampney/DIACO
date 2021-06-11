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
    public class QuejaEstadosController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public QuejaEstadosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los estados de quejas registradas
        /// </summary>
        /// <returns>Listado de los estados de quejas</returns>
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eQuejaEstado>>> GetAll()
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaEstado>("sp_GetAllQuejaEstados");

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de un estado de queja
        /// </summary>
        /// <param name="id">Id del estado de queja</param>
        /// <returns>Objeto con datos del estado de queja</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eQuejaEstado>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eQuejaEstado>("sp_GetQuejaEstados", new { @Id = id });

                if (obj == null)
                    return NotFound();
                else
                    return obj;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Guarda un estado de queja
        /// </summary>
        /// <param name="obj">Objeto con los datos del estado</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromBody] eQuejaEstado obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveQuejaEstados", new { @Id = 0, @Nombre = obj.Nombre, @Inicial = obj.Inicial, @Final = (obj.Final), @Rechazado = (obj.Rechazado) });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de un estado de queja en especifico
        /// </summary>
        /// <param name="id">Id del estado de queja</param>
        /// <param name="obj">Objeto con los datos del estado de queja</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put(int id, [FromBody] eQuejaEstado obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveQuejaEstados", new { @Id = obj.Id, @Nombre = obj.Nombre, @Inicial = obj.Inicial, @Final = obj.Final, @Rechazado = obj.Rechazado });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina un estado de queja en especifico
        /// </summary>
        /// <param name="id">Id del estado de queja</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eQuejaEstado>("sp_GetQuejaEstados", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteQuejaEstados", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de los estados de queja registrados para su seleccion
        /// </summary>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de estados de queja para su seleccion</returns>
        [HttpGet("GetAllValueList/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaEstado>("sp_GetAllQuejaEstados");
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione un estado", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de los estados de queja registrados para su seleccion
        /// </summary>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de estados de queja para su seleccion</returns>
        [HttpGet("GetAllMovimientoValueList/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllMovimientoValueList(bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eQuejaEstado>("sp_GetAllQuejaEstados");
                var valueList = list.Where(b => b.Inicial == false).Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione un estado", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
