using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class RolesController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public RolesController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los roles registrados
        /// </summary>
        /// <returns>Listado de roles</returns>
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eCatalogo>>> GetAll()
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eCatalogo>("sp_GetAllRoles");

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de un rol en especifico
        /// </summary>
        /// <param name="id">Id del rol</param>
        /// <returns>Objeto con datos del rol</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eCatalogo>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eCatalogo>("sp_GetRoles", new { @Id = id });

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
        /// Guarda un rol
        /// </summary>
        /// <param name="obj">Objeto con los datos del rol</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromBody] eCatalogo obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveRoles", new { @Id = 0, @Nombre = obj.Nombre });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de un rol en especifico
        /// </summary>
        /// <param name="id">Id del rol</param>
        /// <param name="obj">Objeto con los datos del rol</param>
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
        /// Elimina un rol en especifico
        /// </summary>
        /// <param name="id">Id del rol</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eCatalogo>("sp_GetRoles", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteRoles", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de los roles registrados para su seleccion
        /// </summary>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de roles para su seleccion</returns>
        [HttpGet("GetAllValueList/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eCatalogo>("sp_GetAllRoles");
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione un rol", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
