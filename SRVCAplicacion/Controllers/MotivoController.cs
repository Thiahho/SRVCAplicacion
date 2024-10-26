using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    public class MotivoController : ControllerBase
    {
     
        private readonly ApplicationDbContext _context;

        public MotivoController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerMotivos()
        {
            try
            {
                var motivos = await _context.Motivo
                          .Select(m => m.nombre_motivo)
                          .ToListAsync();

                return Ok(motivos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> PutDepartamento(int id, [FromBody] Motivo motivo)
        {
            var existe = await _context.Motivo.FindAsync(id);
            if (existe == null)
            {
                return NotFound();
            }
            existe.nombre_motivo = motivo.nombre_motivo;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!motivoExists(id))
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

        private bool motivoExists(int id)
        {
            return _context.Motivo.Any(e => e.id_motivo== id);
        }
    }
}
