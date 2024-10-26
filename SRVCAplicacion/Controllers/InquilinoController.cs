using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquilinoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //public IActionResult GenerarRegistros()
        //{
        //    return View();
        //}

        public InquilinoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("identificacion")]
        public async Task<ActionResult<IEnumerable<string>>> GetIdentificacion()
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
        [HttpGet("obtenerTodos")]
        public async Task<ActionResult<IEnumerable<visitante_inquilino>>> GetVisitantesInquilino()
        {
            return await _context.visitante_Inquilino.ToListAsync();
        }

        [HttpPost("crear")]
        public async Task<ActionResult<visitante_inquilino>> PostVisitanteInquilino(visitante_inquilino visitante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.visitante_Inquilino.Add(visitante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIdentificacion), new { id = visitante.identificacion}, visitante);
        }


        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> PutVisitanteInquilino(int id, visitante_inquilino visitante)
        {
            //if (id != visitante.identificacion)
            //{
            //    return BadRequest();
            //}
            var existe = await _context.visitante_Inquilino.FindAsync(id);

            if (existe == null)
            {
                return NotFound();
            }
            existe.nombre = visitante.nombre;
            existe.apellido=visitante.apellido;
            existe.identificacion = visitante.identificacion;
            existe.activo=visitante.activo;
            existe.telefono=visitante.telefono;
            existe.imgpath=visitante.imgpath;
            existe.estado=visitante.estado;
            //existe.id_visitante_inquilino=visitante.id_visitante_inquilino;
            existe.id_punto_control=visitante.id_punto_control;
            //_context.Entry(visitante).State = EntityState.Modified;

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
        private bool VisitanteInquilinoExists(int id)
        {
            return _context.visitante_Inquilino.Any(e => e.id_visitante_inquilino== id);
        }
    }
}
