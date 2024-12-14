using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRCVShared.Models;
using SRCVShared.Data;

namespace SRVCAplicacion.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PuntosController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;


        public PuntosController(ApplicationDbContext appDb)
        {
            _appDbContext = appDb;
        }

        //[HttpGet("PuntosActivos")]
        //public IActionResult PuntosActivos()
        //{
        //    return View();
        //}

        //[HttpGet("EditarPuntos")]
        //public IActionResult EditarPuntos()
        //{
        //    return View();
        //}

        [HttpGet("ObtenerPuntos")]
        public async Task<ActionResult<IEnumerable<Puntos_de_controles>>> GetPuntos()
        {
            return await _appDbContext.Puntos_de_controles.ToListAsync();
        }

        [HttpPost("CrearPunto")]
        public async Task<IActionResult> PostVisitanteInquilino(Puntos_de_controles puntos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _appDbContext.Puntos_de_controles.AddAsync(puntos);
                await _appDbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPuntos), new { id = puntos.id_punto_control }, puntos);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> PutPuntos(int id, Puntos_de_controles punto)
        {
            if (id != punto.id_punto_control)
            {
                return BadRequest();
            }

            _appDbContext.Entry(punto).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuntoExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        private bool PuntoExiste(int id)
        {
            return _appDbContext.Puntos_de_controles.Any(e => e.id_punto_control == id);
        }


    }
}