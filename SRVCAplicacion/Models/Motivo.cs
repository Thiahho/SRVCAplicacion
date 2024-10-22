using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Motivo
    {
        [Key]
        public int id_motivo { get; set; } = 0;
        [MaxLength(150)]
        public string nombre_motivo{ get; set; } = "";
        public int id_punto_control { get; set; } = 0;

    }
}
