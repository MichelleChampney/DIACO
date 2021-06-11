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
    public class MunicipiosController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public MunicipiosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los municipios registrados
        /// </summary>
        /// <param name="idRegion">id de la region</param>
        /// <param name="idDepartamento">id del departamento</param>
        /// <returns>Listado de Municipios</returns>
        [HttpGet("GetAll/{idRegion}/{idDepartamento}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eCatalogo>>> GetAll(int idRegion, int idDepartamento)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eCatalogo>("sp_GetAllMunicipiosByDepartamento", new { @IdRegion = idRegion, @IdDepartamento = idDepartamento });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de un municipio en especifico
        /// </summary>
        /// <param name="id">Id del municipio</param>
        /// <returns>Objeto con datos del municipio</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eMunicipioVista>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eMunicipioVista>("sp_GetMunicipios", new { @Id = id });

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
        /// <param name="obj">Objeto con los datos del municipio</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromBody] eMunicipio obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveMunicipios", new { @Id = 0, @Nombre = obj.Nombre, @IdDepartamento = obj.IdDepartamento });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de un municipio en especifico
        /// </summary>
        /// <param name="id">Id del municipio</param>
        /// <param name="obj">Objeto con los datos del municipio</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put(int id, [FromBody] eMunicipio obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveMunicipios", new { @Id = obj.Id, @Nombre = obj.Nombre, @IdDepartamento = obj.IdDepartamento });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina un municipio en especifico
        /// </summary>
        /// <param name="id">Id del municipio</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eMunicipio>("sp_GetMunicipios", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteMunicipios", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de los municipios registrados para su seleccion
        /// </summary>
        /// <param name="idRegion">id de la region</param>
        /// <param name="idDepartamento">id del departamento</param>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de municipios para su seleccion</returns>
        [HttpGet("GetAllValueList/{idRegion}/{idDepartamento}/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(int idRegion, int idDepartamento, bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eDepartamento>("sp_GetAllMunicipiosByDepartamento", new { @IdRegion = idRegion, @IdDepartamento = idDepartamento });
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione una municipio", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
