using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Services
{
    public interface ILogAudService
    {
        Task RegistrarCambio(log_aud log);
    }

    public class AuditoriaService : ILogAudService
    {
        private readonly ApplicationDbContext _context;

        public AuditoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarCambio(log_aud log)
        {
            await _context.log_Aud.AddAsync(log);
            await _context.SaveChangesAsync();
        }



    }
}
