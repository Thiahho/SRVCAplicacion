using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRCVShared.Models;
using SRCVShared.Data;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : Controller
    {
        private readonly ApplicationDbContext appDbContext;


        public DepartamentoController(ApplicationDbContext context)
        {
            appDbContext = context;
        }

        [HttpGet("Departamentos")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerDepas()
        {
            try
            {
                var dp = await appDbContext.Departamento
                            .Select(m => new SelectListItem
                            {
                                Value = m.id_dp.ToString(),
                                Text = m.departamento
                            })
                            .ToListAsync();

                return Ok(dp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> PostDepartamentos([FromBody] Departamento depa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await appDbContext.Departamento.AddAsync(depa);
                await appDbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(ObtenerDepas), new { id = depa.id_dp }, depa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
