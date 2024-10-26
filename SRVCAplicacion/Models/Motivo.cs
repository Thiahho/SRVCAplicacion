using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Motivo
    {
        [Key]
        public int id_motivo { get; set; } // ID del motivo

        [MaxLength(45)]
        public string nombre_motivo { get; set; } // Nombre del motivo

        public int id_punto_control { get; set; } // ID del punto de control
    }

}
