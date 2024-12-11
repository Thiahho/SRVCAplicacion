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

        [HttpGet("listaPuntos")]
        public IActionResult listaPuntos()
        {
            return View("~/Views/Puntos/listaPuntos.cshtml");

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
        //--------------inquinilo/Visitas/Registro------------

        [HttpGet("Visitas")]
        public IActionResult Visitas()//visitas
        {
           return View("~/Views/Visitas/Visitas.cshtml");

        }

        [HttpGet("registroIngresos")]
        public IActionResult registroIngresos()
        {
            return View("~/Views/Visitas/registroIngresos.cshtml");

        }

        //-----------------------------BUSQUEDA-----------
        [HttpGet("Busqueda")]
        public IActionResult GenerarRegistros()
        {
            return View("~/Views/Busqueda/GenerarRegistro.cshtml");
        }

        //-----------------------------LOG-----------
        [HttpGet("Log_aud")]
        public IActionResult Log_aud()//CambiosLOG
        {
            return View("~/Views/Log_aud/Log_aud.cshtml");

        }
        //---------------------inquilino---------
        [HttpGet("Inquilinos")]
        public IActionResult Inquilino()//Inquilinos
        {
            return View("~/Views/Inquilino/Inquilino.cshtml");

        }
        [HttpGet("registroInquilinos")]
        public IActionResult registroInquilinos()
        {
            return View("~/Views/Inquilino/registroInquilinos.cshtml");

        }


    }

}
