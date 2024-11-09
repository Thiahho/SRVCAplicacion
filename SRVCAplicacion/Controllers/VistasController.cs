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
        //--------------PUNTOS------------
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
        //--------------USUARIO------------

        [HttpGet("listaUsuario")]
        public IActionResult listaUsuario()
        {
            return View("~/Views/Usuario/listaUsuario.cshtml");
        }
        

        [HttpGet("editarUsuario")]
        public IActionResult editarUsuario()
        {
            return View("~/Views/Usuario/editarUsuario.cshtml");

        }

        [HttpGet("agregarUsuario")]
        public IActionResult agregarUsuario()
        {
            return View("~/Views/Usuario/agregarUsuario.cshtml");

        }
        //--------------inquinilo------------

        [HttpGet("Inquilinos")]
        public IActionResult Visitas()//Inquilinos
        {
           return View("~/Views/Visitas/Visitas.cshtml");

        }

        //-----------------------------BUSQUEDA-----------
        [HttpGet("Busqueda")]
        public IActionResult GenerarRegistros()
        {
            return View("~/Views/Busqueda/GenerarRegistro.cshtml");
        }
    }


}
