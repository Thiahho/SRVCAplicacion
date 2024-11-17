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
        [HttpGet("listaPuntos")]
        public IActionResult listaPuntos()
        {
            return View("~/Views/Puntos/listaPuntos.cshtml");

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

        [HttpGet("listaInquilino")]
        public IActionResult listaInquilino()
        {
           return View("~/Views/Inquilinos/listaInquilino.cshtml");

        }
        [HttpGet("editarInquilino")]
        public IActionResult editarInquilino()
        {
            return View("~/Views/Inquilinos/editarInquilino.cshtml");
        }
        [HttpGet("agregarInquilino")]
        public IActionResult agregarInquilino()
        {
            return View("~/Views/Inquilinos/agregarInquilino.cshtml");
        }
        //-----------------------------BUSQUEDA-----------
        [HttpGet("GenerarRegistros")]
        public IActionResult GenerarRegistros()
        {
            return View("~/Views/Historial/GenerarRegistro.cshtml");
        }


        //--------------REGISTROS------------

        [HttpGet("HistorialRegistros")]
        public IActionResult HistorialRegistros()
        {
            return View("~/Views/Historial/HistorialRegistros.cshtml");

        }
    
    }

}
