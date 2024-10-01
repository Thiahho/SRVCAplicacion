using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Models;
using SRVCAplicacion.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SRVCAplicacion.Controllers
{
    public class AccesoController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        public AccesoController(ApplicationDbContext dbContext)
        {
            _appDbContext = dbContext;
        }
        [HttpGet]

        public IActionResult Registro()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewData["mensaje"] = "los datos no son validos.";
                return View(usuario);
            }
            if (usuario.pass != usuario.CofirmarPass)
            {
                ViewData["mensaje"] = "las contraseñas no coinciden.";
                return View();

            }
            var emailexiste = await _appDbContext.Usuario.FirstOrDefaultAsync(u => u.email == usuario.email);
            if (emailexiste != null)
            {
                ViewData["mensaje"] = "el email ya existe.";
                return View();
            }

            Usuario user = new Usuario()
            {
                email = usuario.email,
                usu = usuario.usu,
                pass = usuario.pass,
                //tipo = usuario.tipo,
            };

            await _appDbContext.Usuario.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            if (user.id != 0) return RedirectToAction("Login", "Acceso");

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
            Usuario? usuario = await _appDbContext.Usuario
                .Where(u => u.usu == login.usuario && u.pass == login.contraseña)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                ViewData["mensaje"] = "No se encontró el usuario.-f";
                return View();
            }

            // Corrige cómo se añade el Claim
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.usu) // Asegúrate de que 'usuario.usu' tenga el valor correcto
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return RedirectToAction("Index", "Home");
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
