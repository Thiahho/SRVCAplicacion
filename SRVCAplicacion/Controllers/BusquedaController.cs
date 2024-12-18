﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SRCVShared.Models;
using SRCVShared.Data;
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

        //chat
        [HttpGet("obtener-todos")]
        public async Task<ActionResult<IEnumerable<registro_visitas>>> ObtenerTodosLosRegistros()
        {
            try
            {
                var registros = await _appDbContext.registro_Visitas
                    .AsNoTracking() // Evita el seguimiento de cambios, útil para mejorar el rendimiento y evitar problemas con EF
                    .ToListAsync();

                if (!registros.Any())
                {
                    return NotFound("No se encontraron registros.");
                }

                return Ok(registros);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocurrió un error: {ex.Message}");
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

        
        [HttpPut("ActualizarHoraSalidaInquilino")]
        public async Task<IActionResult> ActualizarHoraSalidaInquilino([FromQuery] int idRegistro)
        {


            //if (idRegistro == null)
            //{
            //    return BadRequest("El id no puede estar vacío.");
            //}

            //// Buscar el registro asociado al id_registro_vissitas
            //var registro = await _appDbContext.registro_Visitas.FirstOrDefaultAsync(r => r.id_registro_visitas == idRegistro);
            //if (registro == null)
            //{
            //    return NotFound("No se encontró un registro asociado al id proporcionado.");
            //}

            //// Actualizar la hora de salida
            //registro.hora_salida = DateTime.UtcNow;

            //    try
            //    {
            //        await _appDbContext.SaveChangesAsync(); // Guardar cambios en la base de datos
            //    }
            //    catch (Exception ex)
            //    {
            //        return StatusCode(500, $"Error al actualizar el registro: {ex.Message}");
            //    }

            //    return Ok("Hora de salida actualizada correctamente.");
            //}
            //try
            //{
            //    _appDbContext.Entry(registro).Property(r => r.hora_salida).IsModified = true;
            //    await _appDbContext.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Error al actualizar el registro: {ex.Message}");
            //}

            //return Ok("Hora de salida actualizada correctamente.");
            var rowsAffected = await _appDbContext.Database.ExecuteSqlInterpolatedAsync(
                //$"UPDATE registro_visitas SET hora_salida = {DateTime.UtcNow} WHERE id_registro_visitas = {idRegistro}");
                $"UPDATE registro_visitas SET hora_salida = {DateTime.UtcNow}, estado_visita = 2, estado_actualizacion = 2 WHERE id_registro_visitas = {idRegistro}");
            //estado:_visita 1=visita. 2=inquilino

            if (rowsAffected == 0)
            {
                return NotFound("No se encontró el registro para actualizar.");
            }

            return Ok("Hora de salida actualizada correctamente.");
        }
        [HttpPut("ActualizarHoraSalidaVisita")]
        public async Task<IActionResult> ActualizarHoraSalidaVisita([FromQuery] int idRegistro)
        {

            var rowsAffected = await _appDbContext.Database.ExecuteSqlInterpolatedAsync(
                //$"UPDATE registro_visitas SET hora_salida = {DateTime.UtcNow} WHERE id_registro_visitas = {idRegistro}");
                $"UPDATE registro_visitas SET hora_salida = {DateTime.UtcNow}, estado_visita = 1, estado_actualizacion = 2 WHERE id_registro_visitas = {idRegistro}");
            //estado:_visita 1=visita. 2=inquilino

            if (rowsAffected == 0)
            {
                return NotFound("No se encontró el registro para actualizar.");
            }

            return Ok("Hora de salida actualizada correctamente.");
        }




    }
}

