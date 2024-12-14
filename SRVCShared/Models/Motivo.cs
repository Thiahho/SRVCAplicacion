using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRCVShared.Models
{
    [Table("motivo")]

    public class Motivo
    {
        [Key]
        public int id_motivo { get; set; } // ID del motivo

        [MaxLength(45)]
        public string nombre_motivo { get; set; } // Nombre del motivo

        public int id_punto_control { get; set; } // ID del punto de control
    }

}
