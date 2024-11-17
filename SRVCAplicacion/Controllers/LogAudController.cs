using Microsoft.AspNetCore.Mvc;
using SRVCAplicacion.Data;

namespace SRVCAplicacion.Controllers
{
    [Route("api/[controller]")]
    public class LogAudController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;

        public LogAudController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;            
        }


        
    }
}
