﻿using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SRCVShared.Data;
using SRCVShared.Models;

public class Worker : BackgroundService
{
    private readonly ApplicationDbContext _dbworkcontext;
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _outputPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedFiles");

    public Worker(ILogger<Worker> logger, ApplicationDbContext dbContext, HttpClient cliente, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _dbworkcontext = dbContext;
        _httpClient = cliente;
        _serviceProvider = serviceProvider;
    }
    //public async Task<string> GenerarJsonAsync(string filePath)
    //{
    //    try
    //    {
    //        // Obtener los datos desde el DbContext
    //        var usuarios = await _dbworkcontext.Usuario.ToListAsync();
    //        var registros = await _dbworkcontext.registro_Visitas.ToListAsync();
    //        var inquilino = await _dbworkcontext.visitante_Inquilino.ToListAsync();
    //        var logaud = await _dbworkcontext.log_Aud.ToListAsync();
    //        var turnos = await _dbworkcontext.cambio_turno.ToListAsync();

    //        var datosdiccionario = new Dictionary<string, object>
    //        {
    //            {
    //                "Usuarios", usuarios
    //            },
    //            {
    //                "registro_visitas", registros
    //            },
    //            {
    //                "visitante_inquilino", inquilino
    //            },
    //            {
    //                "log_aud", logaud
    //            },
    //            {
    //                "cambios_turnos", turnos
    //            },

    //        };

    //        var jsonData = JsonSerializer.Serialize(datosdiccionario, new JsonSerializerOptions { WriteIndented = true });

    //        await File.WriteAllTextAsync(filePath, jsonData);

    //        _logger.LogInformation("Archivo JSON generado: {filePath}", filePath);

    //        return filePath;


    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error generando el JSON.");
    //        throw;
    //    }
    //}
    private async Task GenerarJsonAsync()
    {
        try
        {
            // Obtener los datos desde el DbContext
            var usuarios = await _dbworkcontext.Usuario.ToListAsync();
            var registros = await _dbworkcontext.registro_Visitas.ToListAsync();
            var inquilino = await _dbworkcontext.visitante_Inquilino.ToListAsync();
            var logaud = await _dbworkcontext.log_Aud.ToListAsync();
            var turnos = await _dbworkcontext.cambio_turno.ToListAsync();

            // Crear diccionario con los datos
            //var datosdiccionario = new Dictionary<string, object>
            //{
            //    { "Usuarios", usuarios },
            //    { "registro_visitas", registros },
            //    { "visitante_inquilino", inquilino },
            //    { "log_aud", logaud },
            //    { "cambios_turnos", turnos }
            //};
            var data = new
            {
                Usuario = usuarios,
                registro_visitas = registros,
                visitante_inquilino = inquilino,
                log_aud = logaud,
                cambio_turno = turnos,
            };

            // Serializar datos a JSON
            //var jsonData = JsonSerializer.Serialize(datosdiccionario, new JsonSerializerOptions { WriteIndented = true });
            var jsonContent = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

            if (!Directory.Exists(_outputPath))
            {
                Directory.CreateDirectory(_outputPath);
            }

            // Guardar el archivo JSON
            var filePath = Path.Combine(_outputPath, $"data_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            await File.WriteAllTextAsync(filePath, jsonContent);

            Console.WriteLine($"Archivo JSON generado: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    _logger.LogInformation("Worker Iniciado.");

    //    while (stoppingToken.IsCancellationRequested)
    //    {
    //        try
    //        {
    //            var filePath = Path.Combine("C:\\data", $"datos_async_{DateTime.UtcNow}.json");
    //            await GenerarJsonAsync(filePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogInformation("Error en sincronizar.", ex);
    //        }
    //        await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
    //    }

    //    _logger.LogInformation("Worker Service error.");

    //}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await GenerarJsonAsync();

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //{
    //    _logger.LogInformation("Worker Service iniciado.");

    //    while (!stoppingToken.IsCancellationRequested)
    //    {
    //        try
    //        {

    //           var usuario = await _dbworkcontext.Usuario.ToListAsync();
    //            var turno = await _dbworkcontext.cambio_turno.ToListAsync();
    //            var registros = await _dbworkcontext.cambio_turno.ToListAsync();
    //            var visitanteInqu = await _dbworkcontext.visitante_Inquilino.ToListAsync();
    //            var puntos = await _dbworkcontext.Puntos_de_controles.ToListAsync();
    //            var logaud = await _dbworkcontext.log_Aud.ToListAsync();

    //            var data = new Dictionary<string, object>
    //            {
    //                {"Usuarios", usuario },
    //                {"cambio_turno", turno},
    //                {"registro_visitas", registros },
    //                {"visitante_inquilino", visitanteInqu }, 
    //                {"puntos_de_controles", puntos },
    //                {"log_aud", logaud },
    //            };
    //            var jsonData= JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true  });
    //            var filePath = Path.Combine("C:\\data", $"datos_sync_{DateTime.Now:yyyyMMdd_HHmmss}.json");
    //            _logger.LogInformation("Archivo generado en {filePath}", filePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error durante el proceso de sincronización.");
    //        }

    //        // Espera 30 minutos antes de volver a ejecutar
    //        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
    //    }

    //    _logger.LogInformation("Worker Service detenido.");
    //}

  
}
