using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Data;

namespace SRVCAplicacion.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly ApplicationDbContext appDbContext;


        public DepartamentoController(ApplicationDbContext context)
        {
            appDbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerDepas()
        {
            // Asumiendo que tienes un modelo "Motivo" con propiedades ID y Nombre
            var dp= await appDbContext.Donde
                            .Select(m => new SelectListItem
                            {
                                Value = m.Id.ToString(),  // El valor del dropdown
                                Text = m.Descripcion           // El texto que se verá
                            })
                            .ToListAsync();

            return Json(dp); // Devuelve los motivos como JSON
        }


    }
}
