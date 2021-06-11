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
    public class RegionesController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public RegionesController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de las regiones registradas
        /// </summary>
        /// <returns>Listado de regiones</returns>
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eCatalogo>>> GetAll()
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eCatalogo>("sp_GetAllRegiones");

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de una region en especifica
        /// </summary>
        /// <param name="id">Id de la region</param>
        /// <returns>Objeto con datos de la region</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eCatalogo>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eCatalogo>("sp_GetRegiones", new { @Id = id });

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
        /// Guarda una region
        /// </summary>
        /// <param name="obj">Objeto con los datos de la region</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromBody] eCatalogo obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveRegiones", new { @Id = 0, @Nombre = obj.Nombre });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de una region en especifico
        /// </summary>
        /// <param name="id">Id de la region</param>
        /// <param name="obj">Objeto con los datos de la region</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put(int id, [FromBody] eCatalogo obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveRegiones", new { @Id = obj.Id, @Nombre = obj.Nombre });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina una region en especifico
        /// </summary>
        /// <param name="id">Id de la region</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eCatalogo>("sp_GetRegiones", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteRegiones", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de las regiones registradas para su seleccion
        /// </summary>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de regiones para su seleccion</returns>
        [HttpGet("GetAllValueList/{valorVacio}/{valor?}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eCatalogo>("sp_GetAllRegiones");
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione una región", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
