﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    public class BusquedaController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        //public IActionResult GenerarRegistros()
        //{
        //    return View();
        //}

        public BusquedaController(ApplicationDbContext applicationDbContext)
        {
            _appDbContext = applicationDbContext;
        }

        [HttpPost("crear")]
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



        //[HttpGet("buscar")]
        //public async Task<ActionResult<List<registro_visitas>>> BuscarRegistro(string condicion, DateTime? desde, DateTime? hasta)
        //{
        //    try
        //    {
        //        var query = _appDbContext.registro_Visitas.AsQueryable();

        //        if (!string.IsNullOrEmpty(condicion))
        //        {
        //            query = query.Where(r => r.nombre_visitante_inquilino.Contains(condicion) ||
        //                                r.nombre_encargado.Contains(condicion) ||
        //                                r.motivo.Contains(condicion) ||
        //                                r.motivo_personalizado.Contains(condicion) ||
        //                                r.depto_visita.Contains(condicion) ||
        //                                r.identificacion_visita.Contains(condicion)
        //                                )
        //             ;

        //        }
        //        //if (desde != DateTime.MinValue && hasta != DateTime.MinValue)
        //        //{
        //        //    query = query.Where(r => r.hora_ingreso >= desde && r.hora_salida <= hasta);
        //        //}
        //        if (desde.HasValue && hasta.HasValue)
        //        {
        //            query = query.Where(r => r.hora_ingreso >= desde.Value && r.hora_salida <= hasta.Value);
        //        }
        //        else if (desde.HasValue)
        //        {
        //            query = query.Where(r => r.hora_ingreso >= desde.Value);
        //        }
        //        else if (hasta.HasValue)
        //        {
        //            query = query.Where(r => r.hora_salida <= hasta.Value);
        //        }

        //        var registros = await query.ToListAsync();
        //        return Ok(registros);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet("buscar")]
        public async Task<ActionResult<List<registro_visitas>>> BuscarRegistro(string condicion, DateTime? desde, DateTime? hasta)
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
                                        r.identificacion_visita.Contains(condicion));
                }

                if (desde.HasValue && hasta.HasValue)
                {
                    query = query.Where(r => r.hora_ingreso >= desde.Value && r.hora_salida <= hasta.Value);
                }

                var registros = await query.ToListAsync();
                return Ok(registros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("obtenerById/{id}")]
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

    }
}
