using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Repository;
using WebApp.Services;

namespace WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class CuentasController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly HashService _hashService;
        public CuentasController(IUserRepository repo, HashService hashService)
        {
            _repo = repo;
            _hashService = hashService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] eUserInfo obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objUsuarioCuenta = JsonConvert.DeserializeObject<eUsuarioCuenta>(_repo.GetCuenta(obj.Email, "Usuarios"));
                    if (objUsuarioCuenta == null) throw new Exception("Usuario no registrado.");
                    var clave = _hashService.Hash(obj.Password, objUsuarioCuenta.Salt);
                    obj.Password = clave.Hash;
                    var objUserToken = JsonConvert.DeserializeObject<eUserToken>(_repo.PostToken(obj, "Cuentas"));
                    await IniciarSesion(objUserToken.Token, objUsuarioCuenta);
                    return RedirectToAction("Index", "Dashboards", new { area = "Admin" });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(obj);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        private async Task IniciarSesion(string token, eUsuarioCuenta obj)
        {
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.Name, obj.Usuario),
                new Claim(ClaimTypes.NameIdentifier, obj.Usuario),
                new Claim(ClaimTypes.Role, obj.NombreRol)
            };

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = true
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal, authProperties);

            HttpContext.Session.SetString("Token", token);
        }
    }
}
