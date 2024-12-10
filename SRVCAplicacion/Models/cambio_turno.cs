using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    [Table("cambio_turno")]
    public class cambio_turno
    {
        [Key]
        public int id_cambio_turno { get; set; } = 0;  // ID de la auditoría

        public int? estado_actualizacion { get; set; } = 0;  // ID del usuario que realiza la acción
        public int id_usuario { get; set; } = 0;  // ID del usuario que realiza la acción
        public int id_punto_control { get; set; } = 0;  // ID del usuario que realiza la acción
        public DateTime? ingreso { get; set; }  // ID del usuario que realiza la acción
        public DateTime? egreso { get; set; }  // ID del usuario que realiza la acción
        public string? observaciones { get; set; } = "";
        public int? activo { get; set; } = 0;  // ID del usuario que realiza la acción


    }
}
