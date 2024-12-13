using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRCVShared.Models;
using SRCVShared.Data;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    public class CambioTurnoController : Controller
    {

        private readonly ApplicationDbContext _appDbContext;

        public CambioTurnoController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("egreso")]
        public async Task<IActionResult> PostCambio([FromBody] cambio_turno cambio)
        {
            var id_punto_controlClaim = User.Claims.FirstOrDefault(c => c.Type == "id_punto_control");
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario");
            if (idUsuarioClaim == null)
            {
                return Unauthorized(new { error = "No se encontró el usuario autenticado." });
            }

            int idUsarioLog = int.Parse(idUsuarioClaim.Value);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Busca el registro correspondiente
                var registroExistente = await _appDbContext.cambio_turno
                    .FirstOrDefaultAsync(ct => ct.id_cambio_turno == cambio.id_cambio_turno);

                if (registroExistente == null)
                {
                    return NotFound(new { error = "No se encontró el registro de cambio de turno." });
                }

                // Actualiza los campos necesarios
                registroExistente.egreso = DateTime.UtcNow; // Fecha de salida
                registroExistente.observaciones = cambio.observaciones; // Si se desea actualizar observaciones

                // Guarda los cambios
                _appDbContext.cambio_turno.Update(registroExistente);
                await _appDbContext.SaveChangesAsync();

                return Ok(new { message = "Egreso registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, innerError = ex.InnerException?.Message });
            }
        }


        //[HttpGet("ultima")]

    }
}
