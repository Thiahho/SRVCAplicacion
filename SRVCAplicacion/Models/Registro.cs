using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Registro
    {
        public int Id { get; set; } = 0;
        [MaxLength(50)]
        public string Identificacion { get; set; } = "";
        [MaxLength(50)]
        public string Nombre { get; set; } = "";
        [MaxLength(50)]
        public string Apellido { get; set; } = "";
        [MaxLength(50)]
        public string Departamento { get; set; } = "";
        public DateTime? HoraDeIngreso { get; set; }
        public DateTime? HoraDeSalida { get; set; }
        [MaxLength(150)]
        public string Motivo { get; set; } = "";
    }
}
