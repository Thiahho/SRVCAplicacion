using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRCVShared.Models;
using SRCVShared.Data;
using SRVWindowsService.Services;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquilinoController : ControllerBase
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


        [HttpGet("ConteoInquilino")]
        public async Task<ActionResult> GetConteoInquilinos()
        {
            try
            {
                int cont = await _context.visitante_Inquilino.CountAsync(i => i.activo == 1 && i.estado==1);
                return Ok(cont);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("contarInquilinosActivos")]
        public async Task<ActionResult> contarInquilinosActivos()
        {
            try
            {
                var activosInquilinos = await _context.visitante_Inquilino
                            .Where(a => a.estado == 1 && a.activo == 1)
                            .CountAsync();
                return Ok(activosInquilinos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "ERROR en el servidor", detalle = ex.Message });
            }
        }
        [HttpGet("contarVisitantesActivos")]
        public async Task<ActionResult> contarVisitantesActivos()
        {
            try
            {
                var activosInquilinos = await _context.visitante_Inquilino
                            .Where(a => a.estado == 2 && a.activo == 1)
                            .CountAsync();
                return Ok(activosInquilinos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "ERROR en el servidor", detalle = ex.Message });
            }
        }



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



        [HttpPost("CrearVisitante")]
        public async Task<IActionResult> PostVisitante([FromBody] visitante_inquilino visitante)
        {
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario");

            //    //Parsea el valor de la claim a int, las claim solo guardan string.
               int idUsarioLog = int.Parse(idUsuarioClaim.Value);

          

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

                    //id_usuario = visitante.id_visitante_inquilino,
                    
                    id_usuario = idUsarioLog,
                    accion = "Creación de visitante",
                    valor_original = null,
                    //valor_nuevo = $"Usuario:{usuario.usuario}, Email:{usuario.email}, Dni:{usuario.dni}, {}",
                    valor_nuevo = $"Nombre:{visitante.nombre}, Dni:{visitante.identificacion}, Activo:{visitante.activo}, Telefono:{visitante.telefono}," +
                                 $"Estado:{visitante.estado}, Punto Control:{visitante.id_punto_control}, Tabla:'visitante_inquilino'",
                    hora = DateTime.UtcNow,
                    id_punto_control = visitante.id_punto_control,
                    estado_actualizacion = 1
                };
                

                // Log para verificar los datos del log
                Console.WriteLine($"Intentando registrar log: {log.valor_nuevo}");

                await _auditoria.RegistrarCambio(log);
                return CreatedAtAction(nameof(GetIdentificacion), new { id = visitante.identificacion }, visitante);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("CrearInquilino")]
        public async Task<IActionResult> PostInquilino([FromBody] visitante_inquilino inquilino)
        {
            // Obtener el valor del claim "id_usuario".
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario");

            //Parsea el valor de la claim a int, las claim solo guardan string.
            int idUsarioLog = int.Parse(idUsuarioClaim.Value);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _context.visitante_Inquilino.AddAsync(inquilino);
                await _context.SaveChangesAsync();

                var log = new log_aud
                {

                    //id_usuario = visitante.id_visitante_inquilino,

                    id_usuario = idUsarioLog,
                    accion = "Creación de inquilino",
                    valor_original = null,
                    //valor_nuevo = $"Usuario:{usuario.usuario}, Email:{usuario.email}, Dni:{usuario.dni}, {}",
                    valor_nuevo = $"Nombre:{inquilino.nombre},Apellido:{inquilino.apellido}, telefono:{inquilino.telefono}, Dni:{inquilino.identificacion}, Activo:{inquilino.activo}, Telefono:{inquilino.telefono}," +
                                 $"Estado:{inquilino.estado}, Punto Control:{inquilino.id_punto_control}, Tabla:'visitante_inquilino'",
                    hora = DateTime.UtcNow,
                    id_punto_control = inquilino.id_punto_control,
                    estado_actualizacion = 1
                };


                // Log para verificar los datos del log
                Console.WriteLine($"Intentando registrar log: {log.valor_nuevo}");

                await _auditoria.RegistrarCambio(log);
                return CreatedAtAction(nameof(GetIdentificacion), new { id = inquilino.identificacion }, inquilino);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //prueba
        [HttpPut("ActualizarInquilinoVisita/{id}")]
        public async Task<IActionResult> ActualizarInquilino(int id, [FromBody] visitante_inquilino visitante_Inquilino)
        {
            // Obtener el valor del claim "id_usuario".
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario");

            //Parsea el valor de la claim a int, las claim solo guardan string.
            int idUsarioLog = int.Parse(idUsuarioClaim.Value);



            if (visitante_Inquilino == null)
            {
                return BadRequest(new { mensaje = "Datos vacios." });
            }

            var inquilinoExistente = _context.visitante_Inquilino.FirstOrDefault(u => u.id_visitante_inquilino == id);

            if (inquilinoExistente == null)
            {
                return NotFound(new { mensaje = $"registro con ID {id} no encontrado." });
            }

            var valorOriginal = $"Inquilino:{inquilinoExistente.nombre},apellido:{inquilinoExistente.apellido},telefono:{inquilinoExistente.telefono},  Dni:{inquilinoExistente.identificacion},Estado:{inquilinoExistente.estado},Activo:{inquilinoExistente.activo}, Punto Control:{inquilinoExistente.id_punto_control}";
            var valorNuevo = $"Inquilino:{visitante_Inquilino.nombre},apellido:{visitante_Inquilino.apellido},telefono:{visitante_Inquilino.telefono}, Dni:{inquilinoExistente.identificacion},Estado:{inquilinoExistente.estado},Activo:{inquilinoExistente.activo}, Punto Control:{inquilinoExistente.id_punto_control}";

            inquilinoExistente.nombre = visitante_Inquilino.nombre;
            inquilinoExistente.apellido = visitante_Inquilino.apellido;
            inquilinoExistente.telefono = visitante_Inquilino.telefono;
            inquilinoExistente.estado_actualizacion = 2;

            await _context.SaveChangesAsync();

            if (inquilinoExistente.estado == 1)
            {
                try
                {
                    
                    var log = new log_aud
                    {
                        id_usuario = idUsarioLog,
                        accion = "Actualización de inquilino",
                        hora = DateTime.UtcNow,
                        valor_original = valorOriginal,
                        valor_nuevo = valorNuevo,
                        //tabla = "Usuario",
                        id_punto_control = 1,
                        estado_actualizacion = 2
                    };
                    if (string.IsNullOrEmpty(log.valor_original) || string.IsNullOrEmpty(log.valor_nuevo))
                    {
                        return NotFound(new { mensaje = "Error al generar el log de auditoría. Los valores originales o nuevos están vacíos." });
                    }

                    //_appDbContext.log_Aud.Add(logAud);

                    await _auditoria.RegistrarCambio(log);

                    return Ok(new { mensaje = "Inquilino actualizado con éxito" });
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(new { mensaje = "Error al guardar los cambios", detalle = ex.Message });
                }
            } else if(inquilinoExistente.estado == 2)
            {
                try
                {

                    var log = new log_aud
                    {
                        id_usuario = idUsarioLog,
                        accion = "Actualización de visitante",
                        hora = DateTime.UtcNow,
                        valor_original = valorOriginal,
                        valor_nuevo = valorNuevo,
                        //tabla = "Usuario",
                        id_punto_control = 1,
                        estado_actualizacion = 2
                    };
                    if (string.IsNullOrEmpty(log.valor_original) || string.IsNullOrEmpty(log.valor_nuevo))
                    {
                        return NotFound(new { mensaje = "Error al generar el log de auditoría. Los valores originales o nuevos están vacíos." });
                    }

                    //_appDbContext.log_Aud.Add(logAud);

                    await _auditoria.RegistrarCambio(log);

                    return Ok(new { mensaje = "Visitante actualizado con éxito" });
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(new { mensaje = "Error al guardar los cambios", detalle = ex.Message });
                }
            }
            else
            {
                return BadRequest(new { mensaje = "error al obtener el estado del registro" });
            }
            
        }
        //asdasd
        [HttpPut("activarActivo/{dni}")]
        public async Task<IActionResult> activarActivo(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return BadRequest(new { mensaje = "El DNI proporcionado no es válido." });
            }

            try
            {
                // Buscar el registro con el DNI proporcionado
                var inquilino = await _context.visitante_Inquilino.FirstOrDefaultAsync(v => v.identificacion == dni);

                if (inquilino == null)
                {
                    return NotFound(new { mensaje = "No se encontró un visitante con el DNI proporcionado." });
                }

                // Actualizar el campo activo
                inquilino.activo = 1;

                await _context.SaveChangesAsync();

                return NoContent(); // Respuesta estándar para una actualización exitosa
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la base de datos.", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado.", error = ex.Message });
            }
        }

        //desactivar activo
        [HttpPut("desactivarActivo/{dni}")]
        public async Task<IActionResult> desactivarActivo(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return BadRequest(new { mensaje = "El DNI proporcionado no es válido." });
            }

            try
            {
                // Buscar el registro con el DNI proporcionado
                var inquilino = await _context.visitante_Inquilino.FirstOrDefaultAsync(v => v.identificacion == dni);

                if (inquilino == null)
                {
                    return NotFound(new { mensaje = "No se encontró un visitante con el DNI proporcionado." });
                }

                // Actualizar el campo activo
                inquilino.activo = 0;

                await _context.SaveChangesAsync();

                return NoContent(); // Respuesta estándar para una actualización exitosa
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la base de datos.", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado.", error = ex.Message });
            }
        }


        //
        [HttpPut("desactivarActivoPRUEBA/{idRegistroSalida}")]
        public async Task<IActionResult> desactivarActivo(int idRegistroSalida)
        {
            if (idRegistroSalida == null)
            {
                return BadRequest(new { mensaje = "El id proporcionado no es válido." });
            }

            try
            {
                // Buscar el registro con el DNI proporcionado
                var registro = await _context.registro_Visitas.FirstOrDefaultAsync(v => v.id_registro_visitas == idRegistroSalida);

                if (registro == null)
                {
                    return NotFound(new { mensaje = "No se encontró un visitante con el DNI proporcionado." });
                }
                var dniDelRegistro = registro.identificacion_visita;

                var inquilino = await _context.visitante_Inquilino.FirstOrDefaultAsync(x => x.identificacion == dniDelRegistro);


                // Actualizar el campo activo
                inquilino.activo = 0;

                await _context.SaveChangesAsync();

                return NoContent(); // Respuesta estándar para una actualización exitosa
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la base de datos.", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado.", error = ex.Message });
            }
        }
        //asdaas
    }


    //[HttpPut("ActualizarInquilino")]
    //public async Task<IActionResult> ActualizarInquilinos(int id, [FromBody] visitante_inquilino visitante_Inquilino)
    //{

    //    if (visitante_Inquilino == null)
    //    {
    //        return BadRequest(new { mensaje = "Datos vacios." });
    //    }

    //    var inquilinoExistente = _context.visitante_Inquilino.FirstOrDefault(u => u.id_visitante_inquilino == id);

    //    if (inquilinoExistente == null)
    //    {
    //        return NotFound(new { mensaje = $"Inquilino con ID {id} no encontrado." });
    //    }

    //    var valorOriginal = $"Inquilino:{inquilinoExistente.nombre}, Dni:{inquilinoExistente.identificacion},Estado:{inquilinoExistente.estado},Activo:{inquilinoExistente.activo}, Punto Control:{inquilinoExistente.id_punto_control}";
    //    var valorNuevo = $"Inquilino:{visitante_Inquilino.nombre}, Dni:{visitante_Inquilino.identificacion},Estado:{visitante_Inquilino.estado},Activo:{visitante_Inquilino.activo}, Punto Control:{visitante_Inquilino.id_punto_control}";

    //    inquilinoExistente.nombre = visitante_Inquilino.nombre;
    //    inquilinoExistente.identificacion = visitante_Inquilino.identificacion;
    //    inquilinoExistente.telefono = visitante_Inquilino.telefono;
    //    inquilinoExistente.estado = visitante_Inquilino.estado;
    //    inquilinoExistente.activo = visitante_Inquilino.activo;
    //    inquilinoExistente.id_punto_control = visitante_Inquilino.id_punto_control;

    //    try
    //    {

    //        var logAud = new log_aud
    //        {
    //            id_usuario = id,
    //            accion = "Actualización de inquilino",
    //            hora = DateTime.Now,
    //            valor_original = valorOriginal,
    //            valor_nuevo = valorNuevo,
    //            //tabla = "Usuario",
    //            id_punto_control = visitante_Inquilino.id_punto_control
    //        };
    //        if (string.IsNullOrEmpty(logAud.valor_original) || string.IsNullOrEmpty(logAud.valor_nuevo))
    //        {
    //            return NotFound(new { mensaje = "Error al generar el log de auditoría. Los valores originales o nuevos están vacíos." });
    //        }

    //        //_appDbContext.log_Aud.Add(logAud);
    //        await _context.SaveChangesAsync();
    //        await _auditoria.RegistrarCambio(logAud);

    //        return Ok(new { mensaje = "Inquilino actualizado con éxito" });
    //    }
    //    catch (DbUpdateException ex)
    //    {
    //        return BadRequest(new { mensaje = "Error al guardar los cambios", detalle = ex.Message });
    //    }
    //}


    //[HttpPut("ActualizarVisita")]
    //public async Task<IActionResult> ActualizarVisita(int id, [FromBody] visitante_inquilino visitante_Inquilino)
    //{

    //    if (visitante_Inquilino == null)
    //    {
    //        return BadRequest(new { mensaje = "Datos vacios." });
    //    }

    //    var inquilinoExistente = _context.visitante_Inquilino.FirstOrDefault(u => u.id_visitante_inquilino == id);

    //    if (inquilinoExistente == null)
    //    {
    //        return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
    //    }

    //    var valorOriginal = $"Usuario:{inquilinoExistente.nombre}, Dni:{inquilinoExistente.identificacion},Estado:{inquilinoExistente.estado},Activo:{inquilinoExistente.activo}, Punto Control:{inquilinoExistente.id_punto_control}";
    //    var valorNuevo = $"Usuario:{visitante_Inquilino.nombre}, Dni:{visitante_Inquilino.identificacion},Estado:{visitante_Inquilino.estado},Activo:{visitante_Inquilino.activo}, Punto Control:{visitante_Inquilino.id_punto_control}";

    //    inquilinoExistente.nombre = visitante_Inquilino.nombre;
    //    inquilinoExistente.identificacion = visitante_Inquilino.identificacion;
    //    inquilinoExistente.telefono = visitante_Inquilino.telefono;
    //    inquilinoExistente.estado = visitante_Inquilino.estado;
    //    inquilinoExistente.activo = visitante_Inquilino.activo;
    //    inquilinoExistente.id_punto_control = visitante_Inquilino.id_punto_control;

    //    try
    //    {

    //        var logAud = new log_aud
    //        {
    //            id_usuario = id,
    //            accion = "Actualización de Visita",
    //            hora = DateTime.Now,
    //            valor_original = valorOriginal,
    //            valor_nuevo = valorNuevo,
    //            //tabla = "Usuario",
    //            id_punto_control = visitante_Inquilino.id_punto_control
    //        };
    //        if (string.IsNullOrEmpty(logAud.valor_original) || string.IsNullOrEmpty(logAud.valor_nuevo))
    //        {
    //            return NotFound(new { mensaje = "Error al generar el log de auditoría. Los valores originales o nuevos están vacíos." });
    //        }

    //        //_appDbContext.log_Aud.Add(logAud);
    //        await _context.SaveChangesAsync();
    //        await _auditoria.RegistrarCambio(logAud);

    //        return Ok(new { mensaje = "Inquilino actualizado con éxito" });
    //    }
    //    catch (DbUpdateException ex)
    //    {
    //        return BadRequest(new { mensaje = "Error al guardar los cambios", detalle = ex.Message });
    //    }
    //}
}
