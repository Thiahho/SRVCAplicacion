using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Service
{
    public interface ILogAudService
    {
        Task RegistroLodAudAsync(LogAud log);
    }

    
}
