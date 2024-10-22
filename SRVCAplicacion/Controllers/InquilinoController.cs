using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Route("api/controller")]
    public class InquilinoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult GenerarRegistros()
        {
            return View();
        }

        public InquilinoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("identificacion")]
        public async Task<ActionResult> GetIdentificacion()
        {
            try
            {
                var ident = await _context.visitante_Inquilino.Select(u => u.identificacion).ToListAsync();
                return Ok(ident);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //por dni y nombre filtro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<visitante_inquilino>>> GetVisitantesInquilino()
        {
            return await _context.visitante_Inquilino.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<visitante_inquilino>> PostVisitanteInquilino(visitante_inquilino visitante)
        {
            _context.visitante_Inquilino.Add(visitante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIdentificacion), new { id = visitante.identificacion}, visitante);
        }


        [HttpPut("{identificacion}")]
        public async Task<IActionResult> PutVisitanteInquilino(string id, visitante_inquilino visitante)
        {
            if (id != visitante.identificacion)
            {
                return BadRequest();
            }

            _context.Entry(visitante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitanteInquilinoExists(id))
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
        private bool VisitanteInquilinoExists(string id)
        {
            return _context.visitante_Inquilino.Any(e => e.identificacion == id);
        }
    }
}
