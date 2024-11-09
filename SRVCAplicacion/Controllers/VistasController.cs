using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Data;

namespace SRVCAplicacion.Controllers
{
    public class VistasController : Controller
    {

        private readonly ApplicationDbContext _context;

        public VistasController(ApplicationDbContext context) 
        {
                _context = context;
        }


            [HttpGet("PuntosActivos")]
            public IActionResult PuntosActivos()
            {
            
                return View("~/Views/Puntos/PuntosActivos.cshtml");
            }

            [HttpGet("EditarPuntos")]
            public IActionResult EditarPuntos()
            {
              return View("~/Views/Puntos/EditarPuntos.cshtml");
            }
            
            [HttpGet("AgregarUsuario")]
            public IActionResult AgregarUsuario()
            {
                return View("~/Views/Usuario/AgregarUsuario.cshtml");
            }

    }

}
