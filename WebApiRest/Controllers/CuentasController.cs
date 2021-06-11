using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiRest.Repository;

namespace WebApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        //private readonly HashService _hashService;//Esto pasarlo al lado de la aplicacion web
        private readonly IGenericRepository _repo;

        public CuentasController(IConfiguration configuration, IGenericRepository repo)
        {
            _configuration = configuration;
            _repo = repo;
        }

        /// <summary>
        /// Valida las credenciales de un usuario
        /// </summary>
        /// <param name="obj">Objeto con los datos del usuario</param>
        /// <returns>Token</returns>
        [HttpPost("PostToken")]
        [AllowAnonymous]
        public async Task<ActionResult<eUserToken>> PostToken([FromBody] eUserInfo obj)
        {
            try
            {
                await _repo.ExecuteScalarSPAsync("sp_ValidarUsuarios", new { @Usuario = obj.Email, @Clave = obj.Password });
                var objCuenta = await _repo.GetSPAsync<eUsuarioCuenta>("sp_GetCuentaUsuarios", new { @Usuario = obj.Email });

                return ArmarToken(obj, objCuenta.NombreRol);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private eUserToken ArmarToken(eUserInfo userInfo, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new eUserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
