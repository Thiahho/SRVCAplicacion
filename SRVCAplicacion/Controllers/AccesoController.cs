using Microsoft.AspNetCore.Mvc;
using SRCVShared.Models;
using SRCVShared.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Text;
using SRVCAplicacion.Services;

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
            if (usuario.clave != usuario.CofirmarPass)
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
                clave = usuario.clave,
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
                .Where(u => u.usuario == login.usuario && u.clave == login.clave)
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
                    new Claim("id_usuario", usu.id_usuario.ToString()),
                    new Claim("id_punto_control", usu.id_punto_control.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                };

                var cambio = new cambio_turno
                {
                    id_usuario = usu.id_usuario,
                    id_punto_control = usu.id_punto_control,
                    ingreso= DateTime.UtcNow,
                    observaciones = $"Usuario   {usu.usuario} inicio sesión",
                    activo = 1,
                    estado_actualizacion = 0
                };

                _appDbContext.cambio_turno.Add(cambio);
                await _appDbContext.SaveChangesAsync();


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

        //[HttpPost("Salir")]
        //public async Task<IActionResult> Salir()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Login", "Acceso");
        //}
        //[HttpPost("Salir")]
        //public async Task<IActionResult> Salir()
        //{
        //    try
        //    {
        //        // Obtener los datos del usuario autenticado desde los claims
        //        var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
        //        var usuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "usuario")?.Value;
        //        var idPuntoControlClaim = User.Claims.FirstOrDefault(c => c.Type == "id_punto_control")?.Value;

        //        if (idUsuarioClaim == null || idPuntoControlClaim == null)
        //        {
        //            return BadRequest(new { error = "No se encontraron los datos del usuario autenticado." });
        //        }

        //        // Parsear los valores
        //        int idUsuario = int.Parse(idUsuarioClaim);
        //        int idPuntoControl = int.Parse(idPuntoControlClaim);

        //        // Crear el objeto cambio_turno
        //        var cambioTurno = new cambio_turno
        //        {
        //            id_usuario = idUsuario,
        //            id_punto_control = idPuntoControl,
        //            egreso = DateTime.UtcNow, // Fecha y hora actual
        //            observaciones = $"Usuario {usuarioClaim} cerró sesión.",
        //            activo = 0 // Marca como inactivo si aplica
        //        };

        //        // Registrar el egreso en la base de datos
        //        await _appDbContext.cambio_turno.AddAsync(cambioTurno);
        //        await _appDbContext.SaveChangesAsync();

        //        // Cerrar sesión
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //        // Redirigir a la página de login
        //        return RedirectToAction("Login", "Acceso");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message, innerError = ex.InnerException?.Message });
        //    }
        //}
        //[HttpPost("Salir")]
        //public async Task<IActionResult> Salir()
        //{
        //    try
        //    {
        //        // Obtener los datos del usuario autenticado desde los claims
        //        var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
        //        var usuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "usuario")?.Value;
        //        var idPuntoControlClaim = User.Claims.FirstOrDefault(c => c.Type == "id_punto_control")?.Value;

        //        if (idUsuarioClaim == null || idPuntoControlClaim == null)
        //        {
        //            return BadRequest(new { error = "No se encontraron los datos del usuario autenticado." });
        //        }

        //        // Parsear los valores
        //        int idUsuario = int.Parse(idUsuarioClaim);
        //        int idPuntoControl = int.Parse(idPuntoControlClaim);

        //        // Buscar el último registro de cambio_turno con egreso null
        //        var cambioTurno = await _appDbContext.cambio_turno
        //            .Where(ct => ct.id_usuario == idUsuario && ct.id_punto_control == idPuntoControl && ct.egreso == null)
        //            .OrderByDescending(ct => ct.id_cambio_turno) // Ordenar por id_cambio_turno descendente (último registro)
        //            .FirstOrDefaultAsync();

        //        if (cambioTurno == null)
        //        {
        //            return BadRequest(new { error = "No se encontró un registro de cambio de turno para actualizar." });
        //        }

        //        // Actualizar el campo egreso con la fecha y hora actual
        //        cambioTurno.egreso = DateTime.UtcNow;
        //        cambioTurno.observaciones = $"Usuario {usuarioClaim} cerró sesión.";
        //        cambioTurno.activo = 0; // Marca como inactivo si aplica

        //        // Guardar los cambios en la base de datos
        //        _appDbContext.cambio_turno.Update(cambioTurno);
        //        await _appDbContext.SaveChangesAsync();

        //        // Cerrar sesión
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //        // Redirigir a la página de login
        //        return RedirectToAction("Login", "Acceso");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message, innerError = ex.InnerException?.Message });
        //    }
        //}
        [HttpPost("Salir")]
        public async Task<IActionResult> Salir()
        {
            try
            {
                // Obtener los datos del usuario autenticado desde los claims
                var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
                var usuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "usuario")?.Value;
                var idPuntoControlClaim = User.Claims.FirstOrDefault(c => c.Type == "id_punto_control")?.Value;

                if (idUsuarioClaim == null || idPuntoControlClaim == null)
                {
                    return BadRequest(new { error = "No se encontraron los datos del usuario autenticado." });
                }

                // Parsear los valores
                int idUsuario = int.Parse(idUsuarioClaim);
                int idPuntoControl = int.Parse(idPuntoControlClaim);

                // Buscar el último registro de cambio_turno con egreso null
                var cambioTurno = await _appDbContext.cambio_turno
                    .Where(ct => ct.id_usuario == idUsuario && ct.id_punto_control == idPuntoControl && ct.egreso == null)
                    .OrderByDescending(ct => ct.id_cambio_turno) // Ordenar por id_cambio_turno descendente (último registro)
                    .FirstOrDefaultAsync();

                if (cambioTurno == null)
                {
                    return BadRequest(new { error = "No se encontró un registro de cambio de turno para actualizar." });
                }

                //// Actualizar el campo egreso con la fecha y hora actual (en UTC)
                //cambioTurno.egreso = DateTime.UtcNow.ToLocalTime();
                cambioTurno.egreso = DateTime.UtcNow;
                cambioTurno.observaciones = $"Usuario {usuarioClaim} cerró sesión.";
                cambioTurno.activo = 0; // Marca como inactivo si aplica

                // Usar transacción para asegurar que la operación sea atómica
                using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Guardar los cambios en la base de datos
                        //_appDbContext.cambio_turno.Update(cambioTurno);
                        await _appDbContext.SaveChangesAsync();

                        // Confirmar la transacción
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        // Si algo falla, hacer rollback
                        await transaction.RollbackAsync();

                        // Devolver el error detallado con la InnerException
                        return BadRequest(new { error = "Error al guardar los cambios", details = ex.InnerException?.Message ?? ex.Message });
                    }
                }

                // Cerrar sesión
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Redirigir a la página de login
                return RedirectToAction("Login", "Acceso");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error al procesar la solicitud", details = ex.InnerException?.Message ?? ex.Message });
            }
        }
        //[HttpPost("Salir")]
        //public async Task<IActionResult> Salir()
        //{
        //    try
        //    {
        //        // Obtener los datos del usuario autenticado desde los claims
        //        var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario")?.Value;
        //        var usuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "usuario")?.Value;
        //        var idPuntoControlClaim = User.Claims.FirstOrDefault(c => c.Type == "id_punto_control")?.Value;

        //        if (string.IsNullOrEmpty(idUsuarioClaim) || string.IsNullOrEmpty(idPuntoControlClaim))
        //        {
        //            var errorMensaje = string.IsNullOrEmpty(idUsuarioClaim)
        //                ? "Claim 'id_usuario' no encontrada."
        //                : "Claim 'id_punto_control' no encontrada.";
        //            return BadRequest(new { error = errorMensaje });
        //        }

        //        // Parsear los valores de forma segura
        //        if (!int.TryParse(idUsuarioClaim, out var idUsuario))
        //        {
        //            return BadRequest(new { error = "El claim 'id_usuario' no es un número válido." });
        //        }

        //        if (!int.TryParse(idPuntoControlClaim, out var idPuntoControl))
        //        {
        //            return BadRequest(new { error = "El claim 'id_punto_control' no es un número válido." });
        //        }

        //        // Verificar que el usuario exista en la base de datos
        //        var usuarioExiste = await _appDbContext.Usuario
        //                                                 .AnyAsync(u => u.id_usuario == idUsuario);
        //        if (!usuarioExiste)
        //        {
        //            return BadRequest(new { error = "El usuario no existe en el sistema." });
        //        }

        //        // Crear el objeto cambio_turno
        //        var cambioTurno = new cambio_turno
        //        {
        //            id_usuario = idUsuario,
        //            id_punto_control = idPuntoControl,
        //            egreso = DateTime.UtcNow, // Fecha y hora actual
        //            observaciones = $"Usuario {usuarioClaim ?? "desconocido"} cerró sesión.",
        //            activo = 0 // Marca como inactivo si aplica
        //        };

        //        // Registrar el egreso en la base de datos
        //        await _appDbContext.cambio_turno.AddAsync(cambioTurno);
        //        await _appDbContext.SaveChangesAsync();

        //        // Cerrar sesión
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //        // Redirigir a la página de login
        //        return RedirectToAction("Login", "Acceso");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Registrar error para monitoreo
        //        Console.WriteLine($"Error en Salir: {ex.Message} - Inner: {ex.InnerException?.Message}");
        //        return BadRequest(new { error = "Ocurrió un error al cerrar sesión.", detalle = ex.Message });
        //    }
        //}



    }
}
