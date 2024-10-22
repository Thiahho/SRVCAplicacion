using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;

namespace SRVCAplicacion.Controllers
{
    [Route("api/controller")]
    public class MotivoController : Controller
    {
     
        private readonly ApplicationDbContext appDbContext;

        public MotivoController(ApplicationDbContext context)
        {
            appDbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerMotivos()
        {
            try
            {
                var motivos = await appDbContext.Motivo
                          .Select(m => new SelectListItem
                          {
                              Value = m.id_motivo.ToString(),  // El valor del dropdown
                              Text = m.nombre_motivo// El texto que se verá
                          })
                          .ToListAsync();

                return Ok(motivos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
