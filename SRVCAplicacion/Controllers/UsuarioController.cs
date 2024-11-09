using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

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
        [HttpGet("Usuarios")]
        public IActionResult listaUsuario()//Index
        {
            return View(listaUsuario);
        }
        //prueba

        [HttpGet("editarUsuario")]
        public IActionResult editarUsuario()
        {
          return View(editarUsuario);
        }
        
        public IActionResult agregarUsuario()
        {
            return View(agregarUsuario);
        }

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

                return CreatedAtAction(nameof(ObtenerUsuarios), new { id = usuario.id_usuario}, usuario);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, innerError = ex.InnerException?.Message });
            }
        }


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
        }
    }
    //asasdasdasdasasdasda
}
