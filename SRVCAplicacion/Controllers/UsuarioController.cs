using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRCVShared.Models;
using SRCVShared.Data;
using SRVCAplicacion.Services;
using System.Reflection.Metadata;
using SRVWindowsService.Services;

namespace SRVCAplicacion.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {

        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogAudService _auditoria;
        public UsuarioController(ApplicationDbContext appdb, ILogAudService logAudService)
        {
            _appDbContext = appdb;
            _auditoria = logAudService;
        }
        //[HttpGet("Usuarios")]
        //public IActionResult listaUsuario()//Index
        //{
        //    return View();
        //}
        ////prueba

        //[HttpGet("editarUsuario")]
        //public IActionResult editarUsuario()
        //{
        //  return View();
        //}

        //public IActionResult agregarUsuario()
        //{
        //    return View();
        //}

        //finprueba

        [HttpGet("obtener")]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObtenerUsuarios()
        {
            try
            {
                var user = await _appDbContext.Usuario.ToListAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> PostUsuarios([FromBody] Usuario usuario)
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

                await _appDbContext.Usuario.AddAsync(usuario);
                await _appDbContext.SaveChangesAsync();

                var log = new log_aud
                {
                    //id_usuario = usuario.id_usuario,
                    id_usuario = idUsarioLog,
                    accion = "Creación de usuario",
                    valor_original = null,
                    //valor_nuevo = $"Usuario:{usuario.usuario}, Email:{usuario.email}, Dni:{usuario.dni}, {}",
                    valor_nuevo = $"Usuario:{usuario.usuario}, Dni:{usuario.dni}, Email:{usuario.email}, clave:{usuario.clave}," +
                                  $"Estado:{usuario.estado}, Punto Control:{usuario.id_punto_control}, Tabla:'usuarios'",
                    hora = DateTime.UtcNow,
                    id_punto_control = usuario.id_punto_control,
                    //tabla = "usuarios"
                };

                await _auditoria.RegistrarCambio(log);
                return CreatedAtAction(nameof(ObtenerUsuarios), new { id = usuario.id_usuario }, usuario);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, innerError = ex.InnerException?.Message });
            }
        }


        //[HttpPut("Actualizar/{id}")]
        //public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        //{
        //    if (id != usuario.id_usuario)
        //    {
        //        return BadRequest();
        //    }

        //    _appDbContext.Entry(usuario).State = EntityState.Modified;

        //    try
        //    {
        //        await _appDbContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
        //        if (!UsuarioExiste(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        //[HttpPut("Actualizar/{id}")]
        //public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
        //{
        //    try
        //    {
        //        var usuarioExiste = await _appDbContext.Usuario.FindAsync(id);

        //        if (usuarioExiste == null)
        //        {
        //            return NotFound("El usuario no existe.");
        //        }
        //        var valorOriginal = $"Usuario: {usuarioExiste.usuario}, Email: {usuarioExiste.email}, Teléfono: {usuarioExiste.telefono}, DNI: {usuarioExiste.dni}," +
        //                            $" Estado: {usuarioExiste.Estado}";


        //        usuarioExiste.usuario = usuario.usuario;
        //        usuarioExiste.email = usuario.email;
        //        usuarioExiste.telefono = usuario.telefono;
        //        usuarioExiste.dni = usuario.dni;
        //        usuarioExiste.clave = usuario.clave;
        //        usuarioExiste.Estado = usuario.Estado;

        //        var valorNuevo = $"Usuario: {usuarioExiste.usuario}, Email: {usuarioExiste.email}, Teléfono: {usuarioExiste.telefono}, DNI: {usuarioExiste.dni}," +
        //                         $" Estado: {usuarioExiste.Estado}";

        //        var registroAudito = new log_aud
        //        {
        //            accion = "Actualizacion de Usuario",
        //            hora = DateTime.UtcNow,
        //            valor_original = valorOriginal,
        //            valor_nuevo = valorNuevo,
        //            tabla = "Usuarios",
        //            id_punto_control = usuarioExiste.id_punto_control
        //        };

        //        _appDbContext.Usuario.Update(usuarioExiste);
        //        _appDbContext.log_Aud.Add(registroAudito);
        //        await _appDbContext.SaveChangesAsync();

        //        return Ok("Usuario actualizado correctamente!");
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpPut("Actualizar/{id}")]
        //public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuarioActualizado)
        //{
        //    try
        //    {
        //        if (usuarioActualizado == null || string.IsNullOrEmpty(usuarioActualizado.usuario) ||
        //            string.IsNullOrEmpty(usuarioActualizado.dni) || string.IsNullOrEmpty(usuarioActualizado.email) ||
        //            string.IsNullOrEmpty(usuarioActualizado.telefono) || string.IsNullOrEmpty(usuarioActualizado.Estado.ToString()))
        //        {
        //            return BadRequest(new { mensaje = "Los datos del usuario no son válidos" });
        //        }

        //        var usuarioOriginal = _appDbContext.Usuario.FirstOrDefault(u => u.id_usuario == id);
        //        if (usuarioOriginal == null)
        //        {
        //            return NotFound(new { mensaje = "Usuario no encontrado" });
        //        }

        //        // Actualizar los valores del usuario original con los nuevos valores
        //        usuarioOriginal.usuario = usuarioActualizado.usuario;
        //        usuarioOriginal.dni = usuarioActualizado.dni;
        //        usuarioOriginal.clave = usuarioActualizado.clave;
        //        usuarioOriginal.email = usuarioActualizado.email;
        //        usuarioOriginal.telefono = usuarioActualizado.telefono;
        //        usuarioOriginal.Estado = usuarioActualizado.Estado;

        //        await _appDbContext.SaveChangesAsync();  // Guardar los cambios en el usuario

        //        // Registrar la auditoría
        //        var registroAuditoria = new log_aud
        //        {
        //            id_usuario = usuarioOriginal.id_usuario,
        //            accion = "Modificación de usuario",
        //            hora = DateTime.UtcNow,
        //            valor_original = string.Join(", ", new string[] {
        //        $"Nombre: {usuarioOriginal.usuario}",
        //        $"DNI: {usuarioOriginal.dni}",
        //        $"clave: {usuarioOriginal.clave}",
        //        $"Email: {usuarioOriginal.email}",
        //        $"Teléfono: {usuarioOriginal.telefono}"
        //    }),
        //            valor_nuevo = string.Join(", ", new string[] {
        //        $"Nombre: {usuarioActualizado.usuario}",
        //        $"DNI: {usuarioActualizado.dni}",
        //        $"clave: {usuarioActualizado.clave}",
        //        $"Email: {usuarioActualizado.email}",
        //        $"Teléfono: {usuarioActualizado.telefono}"
        //    }),
        //            tabla = "usuarios",
        //            id_punto_control = usuarioOriginal.id_punto_control
        //        };

        //        _appDbContext.log_Aud.Add(registroAuditoria);
        //        await _appDbContext.SaveChangesAsync();  // Guardar los cambios de auditoría

        //        return Ok(new { mensaje = "Usuario actualizado con éxito y cambios registrados en auditoría" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { mensaje = "Error al actualizar el usuario", detalle = ex.Message });
        //    }
        //}

        //[HttpPut("Actualizar/{id}")]
        //public async Task<IActionResult> ActualizarUsuario(int id, Usuario usuarioActualizado)
        //{
        //    if (usuarioActualizado == null)
        //    {
        //        return BadRequest(new { mensaje = "Los datos del usuario no son válidos" });
        //    }

        //    // 1. Obtener los datos originales del usuario
        //    var usuarioOriginal = await _appDbContext.Usuario.FindAsync(id);
        //    if (usuarioOriginal == null)
        //    {
        //        return NotFound("Usuario no encontrado.");
        //    }

        //    // 2. Crear una lista para registrar los cambios en la auditoría
        //    var auditoriaList = new List<log_aud>();

        //    // 3. Registrar siempre los valores actuales (aunque no se modifiquen)

        //    // Construir las cadenas para los valores originales y nuevos
        //    string valorOriginalUsuario = $"Usuario: {usuarioOriginal.usuario}, Dni: {usuarioOriginal.dni}, Email: {usuarioOriginal.email}, clave: {usuarioOriginal.clave}, Estado: {usuarioOriginal.Estado}, Punto Control: {usuarioOriginal.id_punto_control}";
        //    string valorNuevoUsuario = $"Usuario: {usuarioActualizado.usuario}, Dni: {usuarioActualizado.dni}, Email: {usuarioActualizado.email}, clave: {usuarioActualizado.clave}, Estado: {usuarioActualizado.Estado}, Punto Control: {usuarioActualizado.id_punto_control}";

        //    Console.WriteLine($"valorOriginalUsuario: {valorOriginalUsuario}");
        //    Console.WriteLine($"valorNuevoUsuario: {valorNuevoUsuario}");

        //    // 4. Registrar la auditoría para todos los campos, incluso si no hay cambios

        //    auditoriaList.Add(new log_aud
        //    {
        //        id_usuario = id,
        //        accion = "Actualización de usuario",
        //        hora = DateTime.UtcNow,
        //        valor_original = valorOriginalUsuario,
        //        valor_nuevo = valorNuevoUsuario,
        //        tabla = "Usuario",
        //        id_punto_control = usuarioActualizado.id_punto_control
        //    });

        //    // 5. Actualizar el usuario con los nuevos valores
        //    usuarioOriginal.usuario = usuarioActualizado.usuario;
        //    usuarioOriginal.dni = usuarioActualizado.dni;
        //    usuarioOriginal.email = usuarioActualizado.email;
        //    usuarioOriginal.contraseña = usuarioActualizado.contraseña;
        //    usuarioOriginal.Estado = usuarioActualizado.Estado;
        //    usuarioOriginal.id_punto_control = usuarioActualizado.id_punto_control;

        //    // 6. Guardar los cambios en el usuario
        //    try
        //    {
        //        await _appDbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { mensaje = $"Error al guardar los cambios: {ex.Message}" });
        //    }

        //    // 7. Registrar la auditoría
        //    await _appDbContext.log_Aud.AddRangeAsync(auditoriaList);
        //    await _appDbContext.SaveChangesAsync();

        //    return Ok(new { mensaje = "Usuario actualizado con éxito" });
        //}

        //OBTIENE BIEN LOS VALORES YA SEAN LOS ORIGINALE Y EL NUEVO PERO A LA HORA DE MANDAR TIRA UN ERROR LA EXCEPCION
        //OBTIENE BIEN LOS VALORES YA SEAN LOS ORIGINALE Y EL NUEVO PERO A LA HORA DE MANDAR TIRA UN ERROR LA EXCEPCION
        //OBTIENE BIEN LOS VALORES YA SEAN LOS ORIGINALE Y EL NUEVO PERO A LA HORA DE MANDAR TIRA UN ERROR LA EXCEPCION
        //OBTIENE BIEN LOS VALORES YA SEAN LOS ORIGINALE Y EL NUEVO PERO A LA HORA DE MANDAR TIRA UN ERROR LA EXCEPCION
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario usuarioActualizado)
        {
            // Obtener el valor del claim "id_usuario".
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario");

            //Parsea el valor de la claim a int, las claim solo guardan string.
            int idUsarioLog = int.Parse(idUsuarioClaim.Value);

            if (usuarioActualizado == null)
            {
                return BadRequest(new { mensaje = "Datos vacios." });
            }

            var usuarioExistente = _appDbContext.Usuario.FirstOrDefault(u => u.id_usuario == id);

            if (usuarioExistente == null)
            {
                return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado." });
            }

            var valorOriginal = $"Usuario:{usuarioExistente.usuario}, Dni:{usuarioExistente.dni}, Email:{usuarioExistente.email}, Contraseña:{usuarioExistente.clave}," +
                $" Estado:{usuarioExistente.estado}, Punto Control:{usuarioExistente.id_punto_control}";
            var valorNuevo = $"Usuario:{usuarioActualizado.usuario}, Dni:{usuarioActualizado.dni}, Email:{usuarioActualizado.email}, Contraseña:{usuarioActualizado.clave}, " +
                $"Estado:{usuarioActualizado.estado}, Punto Control:{usuarioActualizado.id_punto_control}";

            usuarioExistente.usuario = usuarioActualizado.usuario;
            usuarioExistente.clave = usuarioActualizado.clave;
            usuarioExistente.telefono = usuarioActualizado.telefono;
            usuarioExistente.dni = usuarioActualizado.dni;
            usuarioExistente.estado = usuarioActualizado.estado;
            usuarioExistente.email = usuarioActualizado.email;
            usuarioExistente.id_punto_control = usuarioActualizado.id_punto_control;

            try
            {

                var logAud = new log_aud
                {
                    id_usuario = idUsarioLog,
                    accion = "Actualización de usuario",
                    hora = DateTime.Now,
                    valor_original = valorOriginal,
                    valor_nuevo = valorNuevo,
                    //tabla = "Usuario",
                    id_punto_control = usuarioActualizado.id_punto_control,
                     estado_actualizacion = 1
                };
                if (string.IsNullOrEmpty(logAud.valor_original) || string.IsNullOrEmpty(logAud.valor_nuevo))
                {
                    return NotFound(new { mensaje = "Error al generar el log de auditoría. Los valores originales o nuevos están vacíos." });
                }

                //_appDbContext.log_Aud.Add(logAud);
                await _appDbContext.SaveChangesAsync();
                await _auditoria.RegistrarCambio(logAud);

                return Ok(new { mensaje = "Usuario actualizado con éxito" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { mensaje = "Error al guardar los cambios", detalle = ex.Message });
            }
        }




        //[HttpPut("id")]
        //public async Task<IActionResult> PostUsuario(int id, [FromBody] Usuario userActualizado)
        //{
        //    if(id != userActualizado.id_usuario)
        //    {
        //        return BadRequest("El id no existe.");
        //    }

        //    var usuarioOri = await _appDbContext.Usuario.FindAsync(id);
        //    if(usuarioOri == null)
        //    {
        //        return NotFound("Usuario no encontrado.");
        //    }

        //    var log = new List<log_aud>();

        //    if(usuarioOri.usuario != userActualizado.usuario)
        //    {
        //        log.Add(new log_aud
        //        {
        //            id_usuario=id,
        //            accion

        //        })
        //    }
        //}

        //[HttpGet("GetUsuarioActual/{id}")]
        //public IActionResult GetUsuarioActual(int id)
        //{
        //    var usuario = _appDbContext.Usuario
        //        .Where(u => u.id_usuario == id)
        //        .Select(u => new { u.usuario }) // Devuelve solo la columna `usuario`
        //        .FirstOrDefault();

        //    if (usuario == null)
        //    {
        //        return NotFound("Usuario no encontrado.");
        //    }

        //    return Ok(usuario);
        //}
        [HttpGet("GetUsuarioActual/{id}")]
        public IActionResult GetUsuarioActual(int id)
        {
            var usuario = _appDbContext.Usuario
                .Where(u => u.id_usuario == id)
                .Select(u => new
                {
                    u.usuario,
                    u.clave,
                    u.telefono,
                    u.dni,
                    u.estado,
                    u.email,
                    u.id_punto_control
                }) // Devuelve solo los campos necesarios
                .FirstOrDefault();

            if (usuario == null)
            {
                Console.WriteLine($"Usuario con ID {id} no encontrado.");
                return NotFound("Usuario no encontrado.");
            }

            Console.WriteLine($"Usuario encontrado: {usuario.usuario}");
            return Ok(usuario);
        }

        //asasdasdasdasasdasda
        [HttpGet("obtener-claims")]
        public IActionResult ObtenerClaims()
        {
            // Obtener el valor del claim "id_usuario".
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "id_usuario");
            //Parsea el valor de la claim a int, las claim solo guardan string.
            int idUsarioLog = int.Parse(idUsuarioClaim.Value);

            // Obtener el valor del claim "usuario".
            var nombre_encargadoLog = User.Claims.FirstOrDefault(c => c.Type == "usuario")?.Value;

            // Obtener el valor del claim "id_usuario".
            var id_punto_controlClaim = User.Claims.FirstOrDefault(c => c.Type == "id_punto_control");
            //Parsea el valor de la claim a int, las claim solo guardan string.
            int id_punto_controlLog = int.Parse(id_punto_controlClaim.Value);

            return Ok(new { idUsarioLog, nombre_encargadoLog, id_punto_controlLog });
        }
    }
}