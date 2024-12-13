using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRCVShared;
using SRCVShared.Data;
namespace SRVWindowsService.Service
{
    public class WorkService
    {
        private readonly ApplicationDbContext _dbworkcontext;
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _httpClient;
        public WorkService(ApplicationDbContext applicationDbContext, ILogger<Worker> logger, HttpClient httpClient)
        {
            _dbworkcontext = applicationDbContext;
            _logger = logger;
            _httpClient = httpClient;
        }
        //protected override  async Task ExecutarSincronizacionAsync(CancellationToken stopToken)
        //{
        //    while (!stopToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            _logger.LogInformation("Ejecutando servicio :{time}", DateTimeOffset.UtcNow;

        //            await EnviarDatosAsync();

        //            await Task.Delay(TimeSpan.FromMinutes(30), stopToken);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex.Message);
        //        }
        //    }
        //}
        private async Task EnviarDatosAsync()
        {
            //try
            //{
            //    var datos = new 

            //}
        }
        
    }
}
