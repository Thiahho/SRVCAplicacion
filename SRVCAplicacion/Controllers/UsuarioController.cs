using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IWebHostEnvironment webHost;

        public UsuarioController(ApplicationDbContext applicationDb, IWebHostEnvironment environment)
        {
            _appDbContext = applicationDb;
            webHost = environment;
        }

        public IActionResult Index()
        {
            try
            {
                var usuario = _appDbContext.Usuario.OrderByDescending(p => p.id).ToList();
                return View(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public IActionResult Crear()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (usuario.Img == null)
            {
                ModelState.AddModelError("ImgFile", "La imagen del usuario es requerido.");
            }
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(usuario.Img.FileName);
            string fullPath = Path.Combine(webHost.WebRootPath, "usuarios", newFileName);


            using (var stream = System.IO.File.Create(fullPath))
            {
                await usuario.Img.CopyToAsync(stream);
            }

            Usuario user = new Usuario()
            {
                usu = usuario.usu,
                email = usuario.email
                //ImgPath = newFileName
            };
            await _appDbContext.Usuario.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Usuarios", "Usuario");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var user = await _appDbContext.Usuario.FindAsync(id);

            if (user == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            var usuario = new Usuario()
            {
                usu = user.usu,
                email = user.email,
                //ImgPath = user.ImgPath,
            };

            ViewData["id"]=user.id;
            ViewData["usu"] =user.usu;
            ViewData["email"] =user.email;
            //ViewData["ImgPath"] =user.ImgPath;


            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(int id,Usuario usuario)
        {
            var user= await _appDbContext.Usuario.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                ViewData["id"] = user.id;
                ViewData["usu"] = user.usu;
                ViewData["pass"] = user.pass;
                ViewData["email"] = user.email;

                return View(usuario);
            }

            string newImg = user.ImgPath;
            if (user.Img != null)
            {
                newImg = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newImg += Path.GetExtension(usuario.Img.FileName);

                string fullPath=webHost.WebRootPath + "/usuario/" + newImg;
                using (var stream = System.IO.File.Create(fullPath))
                {
                   await usuario.Img.CopyToAsync(stream);
                }
                if (!string.IsNullOrEmpty(user.ImgPath))
                {
                    string oldFullPath = Path.Combine(webHost.WebRootPath , "usuario", user.ImgPath);
                    if (System.IO.File.Exists(oldFullPath))
                    {
                        System.IO.File.Delete(oldFullPath);
                    }
                }
            }

            user.usu = usuario.usu;
            user.pass= usuario.pass;
            user.email = usuario.email;
            user.ImgPath = newImg;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Usuario");


        }

    }
}
