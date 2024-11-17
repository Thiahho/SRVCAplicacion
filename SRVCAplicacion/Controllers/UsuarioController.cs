using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;
using Microsoft.Extensions.Logging;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {

        private readonly ApplicationDbContext _appDbContext;
        
        public UsuarioController(ApplicationDbContext appdb)
        {
            _appDbContext = appdb;
        }
        


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

                return CreatedAtAction(nameof(ObtenerUsuarios), new { id = usuario.id_usuario }, usuario);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, innerError = ex.InnerException?.Message });
            }
        }

        /*
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.id_usuario)
            {
                return BadRequest();
            }

            _appDbContext.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExiste(id))
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
        private bool UsuarioExiste(int id)
        {
            return _appDbContext.Usuario.Any(e => e.id_usuario== id);
        }*/
        
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

    }
    //asasdasdasdasasdasda
}
