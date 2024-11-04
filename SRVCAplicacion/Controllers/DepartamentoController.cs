using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Route("api/controller")]
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
                var dp = await appDbContext.Donde
                            .Select(m => new SelectListItem
                            {
                                Value = m.Id.ToString(),
                                Text = m.Descripcion
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
        public async Task<IActionResult> PostDepartamentos([FromBody] Donde donde)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await appDbContext.Donde.AddAsync(donde);
                await appDbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(ObtenerDepas), new { id = donde.Id }, donde);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
