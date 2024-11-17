using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    [Table("log_aud")]
    public class LogAud
    {
        [Key]
        public int id_log_aud { get; set; }
        public int id_usuario { get; set; }
        public string? accion { get; set; }
        public DateTime hora { get; set; }
        public string? valor_original { get; set; }
        public string? valor_nuevo { get; set; }
        public int id_punto_control { get; set; }
    }
}
