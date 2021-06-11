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
    public class ComerciosController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public ComerciosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los comercios registrados
        /// </summary>
        /// <returns>Listado de comercios</returns>
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eComercio>>> GetAll()
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eComercio>("sp_GetAllComercios");

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de un comercio en especifico
        /// </summary>
        /// <param name="id">Id del comercio</param>
        /// <returns>Objeto con datos del comercio</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eComercio>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eComercio>("sp_GetComercios", new { @Id = id });

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
        /// Guarda un comercio
        /// </summary>
        /// <param name="obj">Objeto con los datos del comercio</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Post([FromBody] eComercio obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveComercios", new { @Id = 0, @NIT = obj.NIT, @Nombre = obj.Nombre, @RazonSocial = obj.RazonSocial, @Telefono = obj.Telefono, @CorreoElectronico = obj.CorreoElectronico });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de un comercio en especifico
        /// </summary>
        /// <param name="id">Id del comercio</param>
        /// <param name="obj">Objeto con los datos del comercio</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put(int id, [FromBody] eComercio obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveComercios", new { @Id = obj.Id, @NIT = obj.NIT, @Nombre = obj.Nombre, @RazonSocial = obj.RazonSocial, @Telefono = obj.Telefono, @CorreoElectronico = obj.CorreoElectronico });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina un comercio en especifico
        /// </summary>
        /// <param name="id">Id del comercio</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eComercio>("sp_GetComercios", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteComercios", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de las Comercios registrados para su seleccion
        /// </summary>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de comercios para su seleccion</returns>
        [HttpGet("GetAllValueList/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eComercio>("sp_GetAllComercios");
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione un comercio", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
