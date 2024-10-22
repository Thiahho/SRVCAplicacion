using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class visitante_inquilino
    {
        [Key]
        public int id_visitante_inquilino { get; set; } = 0;  // ID del visitante o inquilino

        [MaxLength(45)]
        public string nombre { get; set; } = "";  // Nombre del visitante

        [MaxLength(45)]
        public string apellido { get; set; } = "";  // Apellido del visitante

        [MaxLength(45)]
        public string identificacion { get; set; } = "";  // Documento o identificación del visitante

        public int activo { get; set; }  // Estado de actividad: activo o inactivo (enum)

        [MaxLength(45)]
        public string telefono { get; set; } = "";  // Teléfono del visitante

        [MaxLength(45)]
        public string? imgpath { get; set; } = "";  // Ruta de la imagen

        public int estado { get; set; }  // Estado de la visita, dentro o fuera (enum)

        public int id_punto_control { get; set; } = 0;
    }
}
