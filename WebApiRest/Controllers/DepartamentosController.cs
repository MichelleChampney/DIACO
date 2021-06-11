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
    public class DepartamentosController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public DepartamentosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los departamentos registrados
        /// </summary>
        /// <param name="idRegion">id de la region</param>
        /// <returns>Listado de departamentos</returns>
        [HttpGet("GetAll/{idRegion}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eCatalogo>>> GetAll(int idRegion)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eCatalogo>("sp_GetAllDepartamentosByRegion", new { @IdRegion = idRegion });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de un departamento en especifico
        /// </summary>
        /// <param name="id">Id del departamento</param>
        /// <returns>Objeto con datos del departamento</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eDepartamentoVista>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eDepartamentoVista>("sp_GetDepartamentos", new { @Id = id });

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
        /// Guarda un departamento
        /// </summary>
        /// <param name="obj">Objeto con los datos del departamento</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromBody] eDepartamento obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveDepartamentos", new { @Id = 0, @Nombre = obj.Nombre, @IdRegion = obj.IdRegion });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de un departamento en especifico
        /// </summary>
        /// <param name="id">Id del departamento</param>
        /// <param name="obj">Objeto con los datos del departamento</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put(int id, [FromBody] eDepartamento obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveDepartamentos", new { @Id = obj.Id, @Nombre = obj.Nombre, @IdRegion = obj.IdRegion });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina un departamento en especifico
        /// </summary>
        /// <param name="id">Id del departamento</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eDepartamento>("sp_GetDepartamentos", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteDepartamentos", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de los departamentos registrados para su seleccion
        /// </summary>
        /// <param name="idRegion">id de la region</param>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de departmentos para su seleccion</returns>
        [HttpGet("GetAllValueList/{idRegion}/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(int idRegion, bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eDepartamento>("sp_GetAllDepartamentosByRegion", new { @IdRegion = idRegion });
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione un departamento", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
