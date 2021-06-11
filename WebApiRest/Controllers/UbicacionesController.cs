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
    public class UbicacionesController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public UbicacionesController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los ubicaciones registradas
        /// </summary>
        /// <param name="idRegion">id de la region</param>
        /// <param name="idDepartamento">id del departamento</param>
        /// <param name="idMunicipio">id del municipio</param>
        /// <returns>Listado de Ubicaciones</returns>
        [HttpGet("GetAll/{idRegion}/{idDepartamento}/{idMunicipio}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eCatalogo>>> GetAll(int idRegion, int idDepartamento, int idMunicipio)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eCatalogo>("sp_GetAllUbicacionesByMunicipio", new { @IdRegion = idRegion, @IdDepartamento = idDepartamento, @IdMunicipio = idMunicipio });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de una ubicacion en especifico
        /// </summary>
        /// <param name="id">Id de la ubicacion</param>
        /// <returns>Objeto con datos de la ubicacion</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eUbicacionVista>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eUbicacionVista>("sp_GetUbicaciones", new { @Id = id });

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
        /// Guarda un municipio
        /// </summary>
        /// <param name="obj">Objeto con los datos de la ubicacion</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromBody] eUbicacion obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveUbicaciones", new { @Id = 0, @Nombre = obj.Nombre, @IdMunicipio = obj.IdMunicipio });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de una ubicacion en especifico
        /// </summary>
        /// <param name="id">Id de la ubicacion</param>
        /// <param name="obj">Objeto con los datos de la ubicacion</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put(int id, [FromBody] eUbicacion obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveUbicaciones", new { @Id = obj.Id, @Nombre = obj.Nombre, @IdMunicipio = obj.IdMunicipio });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina una ubicacion en especifico
        /// </summary>
        /// <param name="id">Id de la ubicacion</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eUbicacion>("sp_GetUbicaciones", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteUbicaciones", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de los ubicaciones registrados para su seleccion
        /// </summary>
        /// <param name="idRegion">id de la region</param>
        /// <param name="idDepartamento">id del departamento</param>
        /// <param name="idMunicipio">id del municipio</param>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de ubicaciones para su seleccion</returns>
        [HttpGet("GetAllValueList/{idRegion}/{idDepartamento}/{idMunicipio}/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(int idRegion, int idDepartamento, int idMunicipio, bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eMunicipio>("sp_GetAllUbicacionesByMunicipio", new { @IdRegion = idRegion, @IdDepartamento = idDepartamento, @IdMunicipio = idMunicipio });
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione una ubicación", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
