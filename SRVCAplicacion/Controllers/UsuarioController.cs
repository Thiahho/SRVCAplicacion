using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;
using SRVCAplicacion.Services;

namespace SRVCAplicacion.Controllers
{
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
        public async Task<IActionResult> PostUsuarios(Usuario usuario)
        {
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
                    id_usuario = usuario.id_usuario,
                    accion = "Creación de usuario",
                    valor_original = null,
                    //valor_nuevo = $"Usuario:{usuario.usuario}, Email:{usuario.email}, Dni:{usuario.dni}, {}",
                    valor_nuevo = $"Usuario:{usuario.usuario}, Dni:{usuario.dni}, Email:{usuario.email}, Contraseña:{usuario.contraseña}," +
                                  $"Estado:{usuario.estado}, Punto Control:{usuario.id_punto_control}, Tabla:'usuarios'",
                    hora = DateTime.UtcNow,
                    id_punto_control = usuario.id_punto_control,
                    tabla = "usuarios"
                };

                await _auditoria.RegistrarCambio(log);

                return CreatedAtAction(nameof(ObtenerUsuarios), new { id = usuario.id_usuario }, usuario);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, innerError = ex.InnerException?.Message });
            }
        }


        [HttpPut("Actualizar/{id}")]
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
                    id_punto_control = usuarioActualizado.id_punto_control
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
                [HttpGet("GetUsuarioActual/{id}")]
                public IActionResult GetUsuarioActual(int id)
                {
                    var usuario = _appDbContext.Usuario
                        .Where(u => u.id_usuario == id)
                        .Select(u => new
                        {
                            u.usuario,
                            u.contraseña,
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

    }
    //asasdasdasdasasdasda
}
