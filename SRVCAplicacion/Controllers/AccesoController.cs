using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Models;
using SRVCAplicacion.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace SRVCAplicacion.Controllers
{
    //[Route("api/controller")]
    public class AccesoController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        public AccesoController(ApplicationDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public IActionResult Registro()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost("crear")]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Los datos no son válidos.");
                return View(usuario);
            }
            if (usuario.contraseña != usuario.CofirmarPass)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View(usuario);

            }
            var emailexiste = await _appDbContext.Usuario.FirstOrDefaultAsync(u => u.email == usuario.email);
            if (emailexiste != null)
            {
                ModelState.AddModelError(string.Empty, "El email ya existe.");
                return View(usuario);
            }

            Usuario user = new Usuario()
            {
                email = usuario.email,
                usuario = usuario.usuario,
                contraseña = usuario.contraseña,
            };

            await _appDbContext.Usuario.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            if (user.id_usuario != 0) return RedirectToAction("Login", "Acceso");

            ViewData["mensaje"] = "no se pudo crear el usuario.-f";

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                Usuario? usu = await _appDbContext.Usuario
                .Where(u => u.usuario == login.usuario && u.contraseña == login.contraseña)
                .FirstOrDefaultAsync();

                if (usu == null)
                {
                    ViewData["mensaje"] = "No se encontró el usuario.-f";
                    return View();
                }
                if (usu.estado != 1)
                {
                    ViewData["mensaje"] = "El usuario esta Offline.";
                    return View();
                }
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usu.usuario),
                    new Claim("usuario", usu.usuario),
                    //new Claim(ClaimTypes.NameIdentifier, usu.id_usuario.ToString()),
                    new Claim("id_usuario", usu.id_usuario.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                );

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewData["mensaje"] = $"Ocurrió un error: {ex.Message}";
                return View();
            }

            //}
            //[HttpPost("login")]
            //public async Task<IActionResult> Login([FromBody] Login login)
            //{
            //    try
            //    {
            //        // Verificar las credenciales del usuario
            //        var usu = await _appDbContext.Usuario
            //            .Where(u => u.usuario == login.usuario && u.contraseña == login.contraseña)
            //            .FirstOrDefaultAsync();

            //        if (usu == null)
            //        {
            //            return Unauthorized(new { mensaje = "No se encontró el usuario." });
            //        }

            //        if (usu.estado != 1)
            //        {
            //            return Unauthorized(new { mensaje = "El usuario está Offline." });
            //        }

            //        // Crear los claims para el JWT
            //        var claims = new List<Claim>
            //        {
            //            new Claim(ClaimTypes.Name, usu.usuario),
            //            new Claim("usuario", usu.usuario),
            //            new Claim("email", usu.email)
            //        };

            //        // Configurar el secreto y la firma del JWT
            //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //        // Crear el JWT
            //        var token = new JwtSecurityToken(
            //            issuer: _configuration["Jwt:Issuer"],
            //            audience: _configuration["Jwt:Audience"],
            //            claims: claims,
            //            expires: DateTime.Now.AddHours(1),
            //            signingCredentials: credentials
            //        );

            //        var tokenHandler = new JwtSecurityTokenHandler();
            //        var jwtToken = tokenHandler.WriteToken(token);

            //        return Ok(new { Token = jwtToken });
            //    }
            //    catch (Exception ex)
            //    {
            //        return BadRequest(new { mensaje = $"Ocurrió un error: {ex.Message}" });
            //    }
            //}
            //public async Task<IActionResult> Login(Login log  in)
            //{
            //    Usuario? usuario = await _appDbContext.Usuario
            //        .Where(u => u.usu == login.usuario && u.pass == login.contraseña)
            //        .FirstOrDefaultAsync();

            //    if(usuario == null)
            //    {
            //        ViewData["mensaje"] = "No se encontro el usuario.-f";
            //        return View();
            //    }
            //    List<Claim>claims = new List<Claim>();
            //    {
            //        new Claim(ClaimTypes.Name, usuario.usu);
            //    };

            //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            //    AuthenticationProperties properties = new AuthenticationProperties()
            //    {
            //        AllowRefresh = true,
            //    };

            //    await HttpContext.SignInAsync(
            //        CookieAuthenticationDefaults.AuthenticationScheme,
            //        new ClaimsPrincipal(claimsIdentity),
            //        properties
            //        );
            //    return RedirectToAction("Index", "Home");
            //}

        }

        [HttpPost("Salir")]
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}
