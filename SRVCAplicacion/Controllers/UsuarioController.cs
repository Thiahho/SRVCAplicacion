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
        [HttpPut("Actualizar/{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
        {
            try
            {
                var usuarioExiste = await _appDbContext.Usuario.FindAsync(id);

                if (usuarioExiste == null)
                {
                    return NotFound("El usuario no existe.");
                }
                var valorOriginal = $"Usuario: {usuarioExiste.usuario}, Email: {usuarioExiste.email}, Teléfono: {usuarioExiste.telefono}, DNI: {usuarioExiste.dni}, Estado: {usuarioExiste.Estado}";


                usuarioExiste.usuario = usuario.usuario;
                usuarioExiste.email = usuario.email;
                usuarioExiste.telefono = usuario.telefono;
                usuarioExiste.dni = usuario.dni;
                usuarioExiste.contraseña = usuario.contraseña;
                usuarioExiste.Estado = usuario.Estado;

                var valorNuevo = $"Usuario: {usuarioExiste.usuario}, Email: {usuarioExiste.email}, Teléfono: {usuarioExiste.telefono}, DNI: {usuarioExiste.dni}, Estado: {usuarioExiste.Estado}";

                var registroAudito = new log_aud
                {
                    accion = "Actualizacion de Usuario",
                    hora = DateTime.UtcNow,
                    valor_original = valorOriginal,
                    valor_nuevo = valorNuevo,
                    tabla = "Usuarios",
                    id_punto_control = usuarioExiste.id_punto_control
                };

                _appDbContext.Usuario.Update(usuarioExiste);
                _appDbContext.log_Aud.Add(registroAudito);
                await _appDbContext.SaveChangesAsync();

                return Ok("Usuario actualizado correctamente!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
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

        private bool UsuarioExiste(int id)
        {
            return _appDbContext.Usuario.Any(e => e.id_usuario== id);
        }
    }
    //asasdasdasdasasdasda
}
