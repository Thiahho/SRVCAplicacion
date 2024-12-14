using Microsoft.AspNetCore.Http.HttpResults;
using SRCVShared.Data;
namespace SRVCAplicacion.Services
{
    public interface IEnvioDatosService
    {
        Task EnviodeDatos();
    }

    public class EnvioDatosService : IEnvioDatosService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly HttpClient _http;
        public EnvioDatosService(ApplicationDbContext dbContext, HttpClient client)
        {
            _dbContext = dbContext;
            _http = client;

        }

        //private async Task<string> ObtenerDatosSincronizacionAsync(int inpunto)
        //{
        //    try
        //    {
        //        using (var context = new )
        //    } catch (Exception ex) 
        //    {

        //    }
        //}
        public async Task EnviodeDatos()
        {
        }
    }
}
