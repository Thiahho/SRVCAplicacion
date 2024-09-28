using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    [Table("usu")]
    public class Usuario 
    {
        [Column("id")]
        public int Id { get; set; } = 0;
        [Column("email"),MaxLength(50)]
        public string Email { get; set; } = "";
        [Column("usu"),MaxLength(50)]
        public string usuario { get; set; } = "";
        [Column("pass"), MaxLength(50)]
        public string Password { get; set; } = "";
        [NotMapped] 
        public string CofirmarPass{ get; set; } = "";
        [Column("tipo"), MaxLength(5)]
        public int Tipo { get; set; } = 0;
    }
}
