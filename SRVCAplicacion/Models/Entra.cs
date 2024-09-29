using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Entra
    {
        public int Id { get; set; } = 0;
        public DateTime TiempoIn { get; set; }
        public int IdMotivo { get; set; } = 0;
        [MaxLength(50)]
        public string Dni { get; set; } = "";
        public int IdDonde { get; set; } = 0;
    }
}
