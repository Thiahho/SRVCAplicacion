using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [ApiController]
    [Route("api/[controler]")]
    public class LogAudController : Controller
    {

        private readonly ApplicationDbContext _context;

        public LogAudController(ApplicationDbContext app)
        {
            _context = app;
        }


        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarLogAud([FromBody] log_aud registroAuditoria)
        {
            try
            {
                // Valida que el registro no esté vacío
                if (registroAuditoria == null)
                {
                    return BadRequest("Los datos de auditoría no pueden ser nulos.");
                }

                registroAuditoria.hora = DateTime.UtcNow;
                await _context.log_Aud.AddAsync(registroAuditoria);
                await _context.SaveChangesAsync();

                return Ok("Registro de auditoría creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar auditoría: {ex.Message}");
            }
        }
    }
}
