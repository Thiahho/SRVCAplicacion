using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    public class BusquedaController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
       

        public BusquedaController(ApplicationDbContext applicationDbContext)
        {
            _appDbContext = applicationDbContext;
        }
        //[HttpGet("Busqueda")]
        //public IActionResult GenerarRegistros()
        //{
        //    return View();
        //}
        [HttpPost("CrearRegistro")]
        public async Task<ActionResult> CrearRegistro([FromBody] registro_visitas nuevoRegistro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _appDbContext.registro_Visitas.Add(nuevoRegistro);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(nuevoRegistro);
                }
                return BadRequest("Datos Invalidos.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<registro_visitas>> ObtenerRegistroPorId(int id)
        {
            try
            {
                var registro = await _appDbContext.registro_Visitas.FirstOrDefaultAsync(u => u.id_registro_visitas == id);
                if (registro == null)
                {
                    return NotFound();
                }
                return Ok(registro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("BuscarRegistro")]
        public async Task<ActionResult<List<registro_visitas>>> BuscarRegistro(string? condicion, DateTime? desde, DateTime? hasta)
        {
            try
            {
                var query = _appDbContext.registro_Visitas.AsQueryable();

                if (!string.IsNullOrEmpty(condicion))
                {
                    query = query.Where(r => r.nombre_visitante_inquilino.Contains(condicion) ||
                                        r.nombre_encargado.Contains(condicion) ||
                                        r.motivo.Contains(condicion) ||
                                        r.motivo_personalizado.Contains(condicion) ||
                                        r.depto_visita.Contains(condicion) ||
                                        r.identificacion_visita.Contains(condicion)
                                        )
                     ;

                }
                if (desde != DateTime.MinValue && hasta != DateTime.MinValue)
                {
                    query = query.Where(r => r.hora_ingreso >= desde && r.hora_salida <= hasta);
                }

                var registros = await query.ToListAsync();
                return Ok(registros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obtener")]
        public async Task<ActionResult<IEnumerable<registro_visitas>>> ObtenerRegistros()
        {
            try
            {
                var reg = await _appDbContext.registro_Visitas.ToListAsync();
                return Ok(reg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Actualizar/{dni}")]
        public async Task<IActionResult> PutUsuario(string dni, registro_visitas identificacion)
        {
            if (dni != identificacion.identificacion_visita)
            {
                return BadRequest();
            }

            _appDbContext.Entry(identificacion).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroExistes(dni))
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
        private bool RegistroExistes(string dni)
        {
            return _appDbContext.registro_Visitas.Any(e => e.identificacion_visita == dni);
        }

        [HttpPut("ActualizarHorarioSalida/{dni}")]
        public async Task<IActionResult> ActualizarHorarioSalida(string dni, [FromBody] string nuevoHorarioSalida)
        {
            // Intentar convertir el string recibido a un DateTime
            if (!DateTime.TryParse(nuevoHorarioSalida, out DateTime horarioSalida))
            {
                // Devolver un objeto JSON con el mensaje de error
                return BadRequest(new { mensaje = "El horario de salida no tiene un formato válido." });
            }

            // Buscar el registro de visita por el identificador (dni)
            var registro = await _appDbContext.registro_Visitas
                                              .FirstOrDefaultAsync(r => r.identificacion_visita == dni);

            // Si no se encuentra el registro, retornamos NotFound
            if (registro == null)
            {
                return NotFound();
            }

            // Actualizar solo el campo 'hora_salida'
            registro.hora_salida = horarioSalida;

            // Marcar el estado de la entidad como modificado para que se actualice
            _appDbContext.Entry(registro).State = EntityState.Modified;

            try
            {
                // Guardar los cambios en la base de datos
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Si hay un problema de concurrencia, se verifica si el registro aún existe
                if (!RegistroExiste(dni))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Indica que la operación fue exitosa pero no se devuelve contenido
        }
        private bool RegistroExiste(string dni)
        {
            return _appDbContext.registro_Visitas.Any(e => e.identificacion_visita == dni);
        }




        /* original
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest(new { message = "los datos son null." });
            }
            // 1. Validación de modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Si el modelo no es válido, devuelve un 400
            }

            // 2. Verificar que el usuario exista en la base de datos
            var usuarioExistente = await _appDbContext.Usuario.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            // 3. Actualizar solo los campos del usuario que han sido modificados
            // Se asegura de que solo se actualicen los campos que no son nulos o vacíos
            if (!string.IsNullOrEmpty(usuario.usuario))
            {
                usuarioExistente.usuario = usuario.usuario;
            }
            if (!string.IsNullOrEmpty(usuario.email))
            {
                usuarioExistente.email = usuario.email;
            }
            if (!string.IsNullOrEmpty(usuario.telefono))
            {
                usuarioExistente.telefono = usuario.telefono;
            }
            if (!string.IsNullOrEmpty(usuario.dni))
            {
                usuarioExistente.dni = usuario.dni;
            }
            if (!string.IsNullOrEmpty(usuario.contraseña))
            {
                usuarioExistente.contraseña = usuario.contraseña;
            }
            if (usuario.Estado != 0)
            {
                usuarioExistente.Estado = usuario.Estado;
            }
            // 4. Guardar los cambios en la base de datos
            try
            {
                // Cambiar el estado de la entidad a "Modified" para que se actualice en la base de datos
                _appDbContext.Entry(usuarioExistente).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al actualizar el usuario." });
            }
            catch (Exception ex)
            {
                // Para manejar otros posibles errores
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }

            // 5. Devolver una respuesta exitosa
            return NoContent(); // 204: No Content, indicando que la actualización fue exitosa sin cuerpo de respuesta
        }



       */


    }
}
