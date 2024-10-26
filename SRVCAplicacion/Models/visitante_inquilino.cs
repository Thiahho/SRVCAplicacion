using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class visitante_inquilino
    {
        [Key]
        public int id_visitante_inquilino { get; set; } = 0;  // ID del visitante o inquilino

        
        [Required][MaxLength(45)]
        public string nombre { get; set; } = "";  // Nombre del visitante

        [Required]
        [MaxLength(45)]
        public string apellido { get; set; } = "";  // Apellido del visitante

        [Required]
        [MaxLength(45)]
        public string identificacion { get; set; } = "";  // Documento o identificación del visitante

        [Required] public int activo { get; set; }  // Estado de actividad: activo o inactivo (enum)

        [Required]
        [MaxLength(45)]
        public string telefono { get; set; } = "";  // Teléfono del visitante

       
        public string? imgpath { get; set; } = "";  // Ruta de la imagen

        [Required] public int estado { get; set; }  // Estado de la visita, dentro o fuera (enum)

        [Required] public int id_punto_control { get; set; } = 0;
    }
}
