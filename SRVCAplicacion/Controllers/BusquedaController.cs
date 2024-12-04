using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Authorize]
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
        [HttpPost("GenerarRegistro")]
        public async Task<ActionResult> CrearRegistro([FromBody] registro_visitas nuevoRegistro)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Datos Invalidos.");
                }

                var user = User.Identity?.Name;
                nuevoRegistro.nombre_encargado = user;

                _appDbContext.registro_Visitas.Add(nuevoRegistro);

                await _appDbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(CrearRegistro), new { id = nuevoRegistro.id_registro_visitas }, nuevoRegistro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetVisitanteByDni/{dni}")]
        public async Task<ActionResult<visitante_inquilino>> GetVisitanteByDni(string dni)
        {
            var visitante = await _appDbContext.visitante_Inquilino
                                          .FirstOrDefaultAsync(v => v.identificacion == dni);

            if (visitante == null)
            {
                return NotFound("No se encontró un visitante con esa identificación.");
            }

            return Ok(visitante);
        }



        [HttpGet("GetLugares")]
        public async Task<ActionResult<List<Departamento>>> GetLugares()
        {
            var lugares = await _appDbContext.Departamento.ToListAsync();
            return Ok(lugares);
        }

        [HttpGet("GetMotivos")]
        public async Task<ActionResult<List<Motivo>>> GetMotivos()
        {
            var motivos = await _appDbContext.Motivo.ToListAsync();
            return Ok(motivos);
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
        [HttpPut("ActualizarRegistro/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario usuarioActualizado)
        {

            if (usuarioActualizado == null)
            {
                return BadRequest(new { mensaje = "Datos vacios." });
            }

            var usuarioExistente = _appDbContext.Usuario.FirstOrDefault(u => u.id_usuario == id);

            if (usuarioExistente == null)
            {
                return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
            }

            var valorOriginal = $"Usuario:{usuarioExistente.usuario}, Dni:{usuarioExistente.dni}, Email:{usuarioExistente.email}, Contraseña:{usuarioExistente.contraseña}," +
                $" Estado:{usuarioExistente.estado}, Punto Control:{usuarioExistente.id_punto_control}";
            var valorNuevo = $"Usuario:{usuarioActualizado.usuario}, Dni:{usuarioActualizado.dni}, Email:{usuarioActualizado.email}, Contraseña:{usuarioActualizado.contraseña}, " +
                $"Estado:{usuarioActualizado.estado}, Punto Control:{usuarioActualizado.id_punto_control}";

            usuarioExistente.usuario = usuarioActualizado.usuario;
            usuarioExistente.contraseña = usuarioActualizado.contraseña;
            usuarioExistente.telefono = usuarioActualizado.telefono;
            usuarioExistente.dni = usuarioActualizado.dni;
            usuarioExistente.estado = usuarioActualizado.estado;
            usuarioExistente.email = usuarioActualizado.email;
            usuarioExistente.id_punto_control = usuarioActualizado.id_punto_control;

            //try
            //{

            //    var logAud = new log_aud
            //    {
            //        id_usuario = id,
            //        accion = "Actualización de usuario",
            //        hora = DateTime.Now,
            //        valor_original = valorOriginal,
            //        valor_nuevo = valorNuevo,
            //        tabla = "Usuario",
            //        id_punto_control = usuarioActualizado.id_punto_control
            //    };
            //    if (string.IsNullOrEmpty(logAud.valor_original) || string.IsNullOrEmpty(logAud.valor_nuevo))
            //    {
            //        return NotFound(new { mensaje = "Error al generar el log de auditoría. Los valores originales o nuevos están vacíos." });
            //    }

            //_appDbContext.log_Aud.Add(logAud);
            await _appDbContext.SaveChangesAsync();
            //await _auditoria.RegistrarCambio(logAud);

            return Ok(new { mensaje = "Usuario actualizado con éxito" });
        }
        //    catch (DbUpdateException ex)
        //    {
        //        return BadRequest(new { mensaje = "Error al guardar los cambios", detalle = ex.Message });
        //    }
        //}

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




    }
}