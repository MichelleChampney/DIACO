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
    public class UsuariosController : ControllerBase
    {
        private readonly IGenericRepository _repo;

        public UsuariosController(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Trae un listado de los usuarios registrados
        /// </summary>
        /// <returns>Listado de usuarios</returns>
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<eUsuarioVista>>> GetAll()
        {
            try
            {
                var list = await _repo.GetAllSPAsync<eUsuarioVista>("sp_GetAllUsuarios");

                return list.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Trae los datos de un usuario en especifico
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <returns>Objeto con datos del usuario</returns>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult<eUsuarioVista>> Get(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eUsuarioVista>("sp_GetUsuarios", new { @Id = id });

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
        /// Trae la salt de un usuario en especifico
        /// </summary>
        /// <param name="usuario">usuario</param>
        /// <returns>Objeto con datos de la cuenta de usuario</returns>
        [HttpGet("GetCuenta/{usuario}")]
        [AllowAnonymous]
        public async Task<ActionResult<eUsuarioCuenta>> GetCuenta(string usuario)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eUsuarioCuenta>("sp_GetCuentaUsuarios", new { @Usuario = usuario });

                if (obj == null)
                    throw new Exception("Usuario no registrado.");
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
        public async Task<ActionResult> Post([FromBody] eUsuarioCreacion obj)
        {
            try
            {
                await _repo.ExecuteSPAsync("sp_SaveUsuarios", new { @Nombre = obj.Nombre, @Usuario = obj.Usuario, @Clave = obj.Clave, @ConfirmacionClave = obj.ConfirmacionClave, @Salt = obj.Salt, @Activo = obj.Activo, @IdRol = obj.IdRol });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de un usuario en especifico
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <param name="obj">Objeto con los datos del usuario</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Put(int id, [FromBody] eUsuarioActualizacion obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_UpdateUsuarios", new { @Id = obj.Id, @Nombre = obj.Nombre, @Activo = obj.Activo, @IdRol = obj.IdRol });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza los datos de la clave de un usuario en especifico
        /// </summary>
        /// <param name="id">Id del usuario</param>
        /// <param name="obj">Objeto con los datos del usuario</param>
        /// <returns></returns>
        [HttpPut("PutPassword/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> PutPassword(int id, [FromBody] eUsuarioPassword obj)
        {
            try
            {
                if (id != obj.Id)
                    return BadRequest();

                await _repo.ExecuteSPAsync("sp_UpdatePasswordUsuarios", new { @Id = obj.Id, @Clave = obj.Clave, @ConfirmacionClave = obj.ConfirmacionClave, @Salt = obj.Salt });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimina un usuario en especifico
        /// </summary>
        /// <param name="id">Id del rol</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var obj = await _repo.GetSPAsync<eUsuarioCreacion>("sp_GetUsuarios", new { @Id = id });

                if (obj == null)
                    return NotFound();

                await _repo.ExecuteSPAsync("sp_DeleteUsuarios", new { @Id = id });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
