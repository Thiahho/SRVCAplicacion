using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Motivo
    {
        public int Id { get; set; } = 0;
        [MaxLength(150)]
        public string Descripcion { get; set; } = "";

    }
}
