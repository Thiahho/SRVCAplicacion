using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column("Registro"),Key]public int registro { get; set; } = 0;
    }
}
