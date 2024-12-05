using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;
using SRVCAplicacion.Services;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquilinoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogAudService _auditoria;

        public InquilinoController(ApplicationDbContext context, ILogAudService logAudService)
        {
            _context = context;
            _auditoria = logAudService; 
        }

        //[HttpGet("Inquilinos")]
        //public IActionResult Visitas()//Inquilinos
        //{
        //    return View();
        //}
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
        [HttpGet("ObtenerTodos")]
        public async Task<ActionResult<IEnumerable<visitante_inquilino>>> GetVisitantesInquilino()
        {
            return await _context.visitante_Inquilino.ToListAsync();
        }

        [HttpGet("obtener/{identificacionB}")]
        public async Task<ActionResult<visitante_inquilino>> ObtenerVisitante(string identificacionB)
        {
            try
            {
                // Busca el visitante por el numero de dni
                var visitante = await _context.visitante_Inquilino
                    .FirstOrDefaultAsync(v => v.identificacion == identificacionB);

                // Si no se encuentra el visitante, devuelve un error 404
                if (visitante == null)
                {
                    return NotFound(new { message = "Visitante no encontrado" });
                }

                // Si se encuentra, devuelve el visitante
                return Ok(visitante);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devuelve un error 400 con el mensaje de la excepción
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPost("CrearInquilino")]
        public async Task<IActionResult> PostVisitanteInquilino(visitante_inquilino visitante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _context.visitante_Inquilino.AddAsync(visitante);
                await _context.SaveChangesAsync();

                var log = new log_aud
                {
                    id_usuario = visitante.id_visitante_inquilino,
                    accion = "Creación de usuario",
                    valor_original = null,
                    //valor_nuevo = $"Usuario:{usuario.usuario}, Email:{usuario.email}, Dni:{usuario.dni}, {}",
                    valor_nuevo = $"Nombre:{visitante.nombre}, Dni:{visitante.identificacion}, Activo:{visitante.activo}, Telefono:{visitante.telefono}," +
                                 $"Estado:{visitante.estado}, Punto Control:{visitante.id_punto_control}, Tabla:'visitante_inquilino'",
                    hora = DateTime.UtcNow,
                    id_punto_control = visitante.id_punto_control
                };

                await _auditoria.RegistrarCambio(log);
                return CreatedAtAction(nameof(GetIdentificacion), new { id = visitante.identificacion }, visitante);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("{identificacion}")]
        public async Task<IActionResult> PutVisitanteInquilino(int id, visitante_inquilino visitante_Inquilino)
        {
            if (visitante_Inquilino == null)
            {
                return BadRequest(new { mensaje = "Datos vacios." });
            }

            var inquilinoExistente = _context.visitante_Inquilino.FirstOrDefault(u => u.id_visitante_inquilino == id);

            if (inquilinoExistente == null)
            {
                return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
            }

            var valorOriginal = $"Usuario:{inquilinoExistente.nombre}, Dni:{inquilinoExistente.identificacion},Estado:{inquilinoExistente.estado},Activo:{inquilinoExistente.activo}, Punto Control:{inquilinoExistente.id_punto_control}";
            var valorNuevo = $"Usuario:{visitante_Inquilino.nombre}, Dni:{visitante_Inquilino.identificacion},Estado:{visitante_Inquilino.estado},Activo:{visitante_Inquilino.activo}, Punto Control:{visitante_Inquilino.id_punto_control}";

            inquilinoExistente.nombre = visitante_Inquilino.nombre;
            inquilinoExistente.identificacion = visitante_Inquilino.identificacion;
            inquilinoExistente.telefono = visitante_Inquilino.telefono;
            inquilinoExistente.estado = visitante_Inquilino.estado;
            inquilinoExistente.activo = visitante_Inquilino.activo;
            inquilinoExistente.id_punto_control = visitante_Inquilino.id_punto_control;

            try
            {

                var logAud = new log_aud
                {
                    id_usuario = id,
                    accion = "Actualización de usuario",
                    hora = DateTime.Now,
                    valor_original = valorOriginal,
                    valor_nuevo = valorNuevo,
                    tabla = "Usuario",
                    id_punto_control = visitante_Inquilino.id_punto_control
                };
                if (string.IsNullOrEmpty(logAud.valor_original) || string.IsNullOrEmpty(logAud.valor_nuevo))
                {
                    return NotFound(new { mensaje = "Error al generar el log de auditoría. Los valores originales o nuevos están vacíos." });
                }

                //_appDbContext.log_Aud.Add(logAud);
                await _context.SaveChangesAsync();
                await _auditoria.RegistrarCambio(logAud);

                return Ok(new { mensaje = "Inquilino actualizado con éxito" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { mensaje = "Error al guardar los cambios", detalle = ex.Message });
            }
        }
        private bool VisitanteInquilinoExists(string id)
        {
            return _context.visitante_Inquilino.Any(e => e.identificacion == id);
        }

        [HttpGet("ConteoInquilino")]
        protected async Task<ActionResult> GetConteoInquilinos()
        {
            try
            {
                int cont = await _context.visitante_Inquilino.CountAsync(i => i.activo == 1);
                return Ok(cont);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
