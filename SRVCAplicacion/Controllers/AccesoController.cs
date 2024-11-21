//using Microsoft.AspNetCore.Mvc;
//using SRVCAplicacion.Models;
//using SRVCAplicacion.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication; 
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;

//namespace SRVCAplicacion.Controllers
//{
//    //[Route("api/[controller]")]
//    public class AccesoController : Controller
//    {
//        private readonly ApplicationDbContext _appDbContext;
//        private readonly ILogger<AccesoController> _logger;
//        public AccesoController(ApplicationDbContext dbContext, ILogger<AccesoController>logger)
//        {
//            _appDbContext = dbContext;
//            _logger = logger;
//        }

//        public IActionResult Registro()
//        {
//            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
//            return View();
//        }
//        [HttpPost("crear")]
//        public async Task<IActionResult> Registro(Usuario usuario)
//        {
//            if (!ModelState.IsValid)
//            {
//                ModelState.AddModelError(string.Empty, "Los datos no son válidos.");
//                return View(usuario);
//            }
//            if (usuario.contraseña!= usuario.CofirmarPass)
//            {
//                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
//                return View(usuario);

//            }
//            var emailexiste = await _appDbContext.Usuario.FirstOrDefaultAsync(u => u.email == usuario.email);
//            if (emailexiste != null)
//            {
//                ModelState.AddModelError(string.Empty, "El email ya existe.");
//                return View(usuario);
//            }

//            Usuario user = new Usuario()
//            {
//                email = usuario.email,
//                usuario = usuario.usuario,
//                contraseña = usuario.contraseña,
//            };

//            await _appDbContext.Usuario.AddAsync(user);
//            await _appDbContext.SaveChangesAsync();

//            if (user.id_usuario != 0) return RedirectToAction("Login", "Acceso");

//            ViewData["mensaje"] = "no se pudo crear el usuario.-f";

//            return View();
//        }


//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            // Eliminar la sesión activa
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            // Redirigir al login
//            return RedirectToAction("Login", "Acceso");
//        }

//        [HttpGet]
//        public IActionResult Login()
//        {
//            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
//            return View();
//        }

//        //[HttpPost]
//        //public async Task<IActionResult> Login([FromBody]Login login)
//        //{
//        //    try
//        //    {
//        //        if (string.IsNullOrWhiteSpace(login.usuario) || string.IsNullOrWhiteSpace(login.contraseña))
//        //        {
//        //            return Json(new { success = false, message = "El usuario o contraseña no pueden estar vacíos." });
//        //        }

//        //        Usuario? usu = await _appDbContext.Usuario
//        //        .Where(u => u.usuario == login.usuario && u.contraseña == login.contraseña)
//        //        .FirstOrDefaultAsync();

//        //        if (usu == null)
//        //        {
//        //            ViewData["mensaje"] = "No se encontró el usuario.-f";
//        //            return View();
//        //        }
//        //        if(usu.Estado != 1)
//        //        {
//        //            ViewData["mensaje"] = "El usuario esta Offline.";
//        //            return View();
//        //        }
//        //        List<Claim> claims = new List<Claim>
//        //        {
//        //            new Claim(ClaimTypes.Name, usu.usuario),
//        //            new Claim("usuario", usu.usuario) 
//        //        };

//        //        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//        //        AuthenticationProperties properties = new AuthenticationProperties()
//        //        {
//        //            IsPersistent = true,
//        //            ExpiresUtc= DateTimeOffset.UtcNow.AddHours(1),
//        //        };

//        //        await HttpContext.SignInAsync(
//        //            CookieAuthenticationDefaults.AuthenticationScheme,
//        //            new ClaimsPrincipal(claimsIdentity),
//        //            properties
//        //        );

//        //        //return RedirectToAction("Login", "Acceso");
//        //        return RedirectToAction("Index", "Home");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        ViewData["mensaje"] = $"Ocurrió un error: {ex.Message}";
//        //        return View();
//        //    }

//        //}

//        //[HttpPost]
//        //public async Task<IActionResult> Login([FromBody] Login login)
//        //{
//        //    try
//        //    {
//        //        // Validación básica
//        //        if (string.IsNullOrWhiteSpace(login.usuario) || string.IsNullOrWhiteSpace(login.contraseña))
//        //        {
//        //            return Json(new { success = false, message = "El usuario o contraseña no pueden estar vacíos." });
//        //        }

//        //        // Buscar usuario
//        //        Usuario? usu = await _appDbContext.Usuario
//        //                        .Where(u => u.usuario == "admin" && u.contraseña == "adm123")
//        //                        .FirstOrDefaultAsync();

//        //        if (usu == null)
//        //        {
//        //            return Json(new { success = false, message = "No se encontró el usuario." });
//        //        }

//        //        if (usu.Estado != 1)
//        //        {
//        //            return Json(new { success = false, message = "El usuario está offline." });
//        //        }

//        //        // Crear Claims
//        //        List<Claim> claims = new List<Claim>
//        //        {
//        //            new Claim(ClaimTypes.Name, usu.usuario),
//        //            new Claim("usuario", usu.usuario)
//        //        };

//        //        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//        //        AuthenticationProperties properties = new AuthenticationProperties()
//        //        {
//        //            IsPersistent = true,
//        //            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
//        //        };

//        //        // Iniciar sesión
//        //        await HttpContext.SignInAsync(
//        //            CookieAuthenticationDefaults.AuthenticationScheme,
//        //            new ClaimsPrincipal(claimsIdentity),
//        //            properties
//        //        );

//        //        return Json(new { success = true, message = "Inicio de sesión exitoso.", redirectUrl = Url.Action("Index", "Home") });
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Console.WriteLine($"Error en Login: {ex.Message}");
//        //        //return Json(new { success = false, message = $"Ocurrió un error: {ex.Message}" });
//        //        return BadRequest(new { success = false, message = $"Ocurrió un error: {ex.Message}" });
//        //    }
//        //}

//        //ULTIMAPRUEBAAAAA
//        //[HttpPost]
//        //public async Task<IActionResult> Login([FromBody] Login login)
//        //{
//        //    try
//        //    {
//        //        // Verificar si el usuario y la contraseña son correctos
//        //        Usuario? usu = await _appDbContext.Usuario
//        //            .Where(u => u.usuario == login.usuario && u.contraseña == login.contraseña)
//        //            .FirstOrDefaultAsync();

//        //        // Asegurarse de que el usuario fue encontrado
//        //        if (usu == null)
//        //        {
//        //            return BadRequest(new { success = false, message = "Usuario o contraseña incorrectos." });
//        //        }

//        //        // Verificar el estado del usuario
//        //        if (usu.Estado != 1)
//        //        {
//        //            return BadRequest(new { success = false, message = "El usuario está offline." });
//        //        }

//        //        // Si el usuario existe, crear las Claims
//        //        List<Claim> claims = new List<Claim>
//        //{
//        //    new Claim(ClaimTypes.Name, usu.usuario),
//        //    new Claim("usuario", usu.usuario)
//        //};

//        //        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//        //        AuthenticationProperties properties = new AuthenticationProperties()
//        //        {
//        //            IsPersistent = true,
//        //            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
//        //        };

//        //        // Iniciar sesión
//        //        await HttpContext.SignInAsync(
//        //            CookieAuthenticationDefaults.AuthenticationScheme,
//        //            new ClaimsPrincipal(claimsIdentity),
//        //            properties
//        //        );

//        //        return Ok(new { success = true, message = "Inicio de sesión exitoso" });
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        // Captura cualquier error inesperado
//        //        return BadRequest(new { success = false, message = $"Ocurrió un error: {ex.Message}" });
//        //    }
//        //}

//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] Login login)
//        {
//            try
//            {
//                // Log de los datos recibidos
//                _logger.LogInformation($"Intentando iniciar sesión con el usuario: {login.usuario}");

//                Usuario? usu = await _appDbContext.Usuario
//                    .Where(u => u.usuario == login.usuario && u.contraseña == login.contraseña)
//                    .FirstOrDefaultAsync();

//                if (usu == null)
//                {
//                    _logger.LogWarning("No se encontró el usuario.");
//                    ViewData["mensaje"] = "No se encontró el usuario.-f";
//                    return View();
//                }

//                if (usu.Estado != 1)
//                {
//                    _logger.LogWarning($"El usuario {login.usuario} está Offline.");
//                    ViewData["mensaje"] = "El usuario está Offline.";
//                    return View();
//                }

//                List<Claim> claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, usu.usuario),
//                    new Claim("usuario", usu.usuario)
//                };

//                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//                AuthenticationProperties properties = new AuthenticationProperties()
//                {
//                    IsPersistent = true,
//                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
//                };

//                await HttpContext.SignInAsync(
//                    CookieAuthenticationDefaults.AuthenticationScheme,
//                    new ClaimsPrincipal(claimsIdentity),
//                    properties
//                );

//                return RedirectToAction("Index", "Home");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error al intentar iniciar sesión: {ex.Message}", ex);
//                ViewData["mensaje"] = $"Ocurrió un error: {ex.Message}";
//                return View();
//            }
//        }


//        //public async Task<IActionResult> Login(Login log  in)
//        //{
//        //    Usuario? usuario = await _appDbContext.Usuario
//        //        .Where(u => u.usu == login.usuario && u.pass == login.contraseña)
//        //        .FirstOrDefaultAsync();

//        //    if(usuario == null)
//        //    {
//        //        ViewData["mensaje"] = "No se encontro el usuario.-f";
//        //        return View();
//        //    }
//        //    List<Claim>claims = new List<Claim>();
//        //    {
//        //        new Claim(ClaimTypes.Name, usuario.usu);
//        //    };

//        //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
//        //    AuthenticationProperties properties = new AuthenticationProperties()
//        //    {
//        //        AllowRefresh = true,
//        //    };

//        //    await HttpContext.SignInAsync(
//        //        CookieAuthenticationDefaults.AuthenticationScheme,
//        //        new ClaimsPrincipal(claimsIdentity),
//        //        properties
//        //        );
//        //    return RedirectToAction("Index", "Home");
//        //}

//    }
//}
using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Models;
using SRVCAplicacion.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
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
                if (usu.Estado != 1)
                {
                    ViewData["mensaje"] = "El usuario esta Offline.";
                    return View();
                }
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usu.usuario),
                    new Claim("usuario", usu.usuario)
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

        }
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
}