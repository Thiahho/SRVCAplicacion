using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Entra
    {
        protected int Id { get; set; } = 0;
        protected DateTime TiempoIn { get; set; }
        protected int IdMotivo { get; set; } = 0;
        [MaxLength(50)]
        protected string Dni { get; set; } = "";
        protected int IdDonde { get; set; } = 0;
    }
}
