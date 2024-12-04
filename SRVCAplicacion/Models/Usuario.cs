using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    [Table("usuarios")]
    public class Usuario 
    {
        //[Column("id")]
        [Key]
        public int id_usuario { get; set; } = 0;
        public int id_punto_control { get; set; } = 0;
        //[Column("email"), MaxLength(50)]
        public string? usuario { get; set; } = "";
        public string? email { get; set; } = "";
        public string? telefono { get; set; } = "";
        //[Column("usu"), MaxLength(50)]
        public string? dni { get; set; } = "";
        //[Column("pass"), MaxLength(50)]
        public string? contraseña { get; set; } = "";
        [NotMapped] 
        public string CofirmarPass{ get; set; } = "";
        //[Column("tipo")]
        //public int Tipo { get; set; } 
        public int estado { get; set; }
    }

    
}
