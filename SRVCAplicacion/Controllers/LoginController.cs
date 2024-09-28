using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Models;
using SRVCAplicacion.Services;

namespace SRVCAplicacion.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        public LoginController(ApplicationDbContext dbContext)
        {
            _appDbContext = dbContext;
        }
        [HttpGet ]

        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Mensaje"] = "Los datos no son validos.";
                return View(usuario);
            }
            if(usuario.Password != usuario.CofirmarPass)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                return View();

            }
            var emailExiste= await
            Usuario user = new Usuario()
            {
                Email = usuario.Email,
                usuario = usuario.usuario,
                Password = usuario.Password,
                Tipo = usuario.Tipo,
            };

            await _appDbContext.Usuario.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            if (user.Id != 0) return RedirectToAction("Login", "Acceso");

            ViewData["Mensaje"] = "No se pudo crear el usuario.-F";
            
            return View();
        }

    }
}
