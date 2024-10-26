using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public DepartamentoController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerDepas()
        {
            try
            {
                var dp = await _context.Departamento
                          .Select(m => m.departamento)
                          .ToListAsync();

                return Ok(dp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Departamento>> PostDepartamento([FromBody]Departamento dp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Departamento.Add(dp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerDepas), new { id = dp.id_dp}, dp);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> PutDepartamento(int id, [FromBody]Departamento dp)
        {
            var existe = await _context.Departamento.FindAsync(id);
            if(existe == null)
            {
                return NotFound();
            }
            existe.departamento = dp.departamento;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!dpExists(id))
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

        private bool dpExists(int id)
        {
            return _context.Departamento.Any(e => e.id_dp== id);
        }
    }
}
