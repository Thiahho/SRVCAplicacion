using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SRCVShared.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SRVCAplicacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SincronizacionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _outputPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedFiles");

        public SincronizacionController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //[HttpPost("GenerarJSON")]
        //public async Task<IActionResult> Generar()
        //{
        //    try
        //    {
        //        var file = Path.Combine("C:\\Ruta\\Sincronizacion", $"datos_sync_{DateTime.Now:yyyyMMdd_HHmmss}.json");

        //        await new SincronizacionController
        //    }
        //}

        //[HttpPost("manual")]
        //public async Task<IActionResult> ManualAsync()
        //{
        //    try
        //    {
        //        var filePath = Path.Combine("C:\\data", $"datos_manual_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");
        //        await _worker.GenerarJsonAsync(filePath);
        //        return Ok($"Archivo gnerado:{filePath}");

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500,$"Error:{ex.Message}");

        //    }

        //}
        [HttpPost("GenerarJson")]
        public async Task<IActionResult> GenerarJson()
        {
            const string _outputPath = "C:\\Data";
            try
            {
                //var usuarios = await _dbContext.Usuario.ToListAsync();
                //var registros = await _dbContext.registro_Visitas.ToListAsync();
                //var inquilino = await _dbContext.visitante_Inquilino.ToListAsync();
                //var logaud = await _dbContext.log_Aud.ToListAsync();
                //var turnos = await _dbContext.cambio_turno.ToListAsync();
                //var data = new
                //{
                //    Usuario = usuarios,
                //    registro_visitas = registros,
                //    visitante_inquilino = inquilino,
                //    log_aud = logaud,
                //    cambio_turno = turnos,
                //};
                //var usuarios = await _dbContext.Usuario.ToListAsync();
                var registros = await _dbContext.registro_Visitas.ToListAsync();
                var inquilino = await _dbContext.visitante_Inquilino.ToListAsync();
                var logaud = await _dbContext.log_Aud.ToListAsync();
                var turnos = await _dbContext.cambio_turno.ToListAsync();

                var data = new Dictionary<string, object>
                {
                    //{
                    //    "Usuarios", usuarios
                    //},
                    {
                        "registro_visitas", registros
                    },
                    {
                        "visitante_inquilino", inquilino
                    },
                    {
                        "log_aud", logaud
                    },
                    {
                        "cambios_turnos", turnos
                    },

                };
                var jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);

                if (!Directory.Exists(_outputPath))
                {
                    Directory.CreateDirectory(_outputPath);
                }

                var filePath = Path.Combine(_outputPath, $"data_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                await System.IO.File.WriteAllTextAsync(filePath, jsonContent);

                return Ok(new { Message = "Archivo generado correctamente.", Path = filePath });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error al generar archivo JSON.", Error = ex.Message });
            }
        }


    }
}
