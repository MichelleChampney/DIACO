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
    public class ComercioSucursalesController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public ComercioSucursalesController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de las sucursales de los comercios registrados
        /// </summary>
        /// <returns>Listado de sucursales de los comercios</returns>
        [HttpGet("GetAll/{idComercio}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eComercioSucursalVista>>> GetAll(int idComercio)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eComercioSucursalVista>("sp_GetAllComercioSucursalesByComercio", new { @IdComercio = idComercio });

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de una sucursal en especifico
        /// </summary>
        /// <param name="id">Id del comercio</param>
        /// <returns>Objeto con datos de la sucursal</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eComercioSucursalVista>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eComercioSucursalVista>("sp_GetComercioSucursales", new { @Id = id });

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
        public async Task<ActionResult> Post([FromBody] eComercioSucursal obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveComercioSucursales", new { @Id = 0, @IdComercio = obj.IdComercio, @Nombre = obj.Nombre, @IdUbicacion = obj.IdUbicacion, @Direccion = obj.Direccion, @Central = obj.Central, @Telefono = obj.Telefono });

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
        public async Task<ActionResult> Put(int id, [FromBody] eComercioSucursal obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_SaveComercioSucursales", new { @Id = obj.Id, @IdComercio = obj.IdComercio, @Nombre = obj.Nombre, @IdUbicacion = obj.IdUbicacion, @Direccion = obj.Direccion, @Central = obj.Central, @Telefono = obj.Telefono });

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
                var obj = await _repo.GetSPAsync<eComercioSucursal>("sp_GetComercioSucursales", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteComercioSucursales", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae un listado de las ComercioSucursales registrados para su seleccion
        /// </summary>
        /// <param name="idComercio">id del comercio</param>
        /// <param name="valorVacio">true si se desea agregar un primer valor vacio</param>
        /// <param name="valor">valor que se desea cargue seleccionado</param>
        /// <returns>Listado de ComercioSucursales para su seleccion</returns>
        [HttpGet("GetAllValueList/{idComercio}/{valorVacio}/{valor?}")]
        public async Task<ActionResult<IEnumerable<eValueList>>> GetAllValueList(int idComercio, bool valorVacio, string valor)
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eComercioSucursal>("sp_GetAllComercioSucursalesByComercio", new { @IdComercio = idComercio });
                var valueList = list.Select(i => new eValueList
                {
                    Value = i.Id.ToString(),
                    Text = i.Nombre,
                    Selected = (valor == i.Id.ToString())
                });

                if (valorVacio)
                    valueList = valueList.Prepend(new eValueList() { Value = "0", Text = "Seleccione una sucursal", Selected = string.IsNullOrWhiteSpace(valor) });

                return valueList.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
