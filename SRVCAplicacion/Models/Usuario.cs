using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    [Table("usu")]
    public class Usuario 
    {
        //[Column("id")]
        public int id { get; set; } = 0;
        //[Column("email"), MaxLength(50)]
        public string? email { get; set; } = "";
        //[Column("usu"), MaxLength(50)]
        public string? usu { get; set; } = "";
        //[Column("pass"), MaxLength(50)]
        public string? pass { get; set; } = "";
        [NotMapped] 
        public string CofirmarPass{ get; set; } = "";
        //[Column("tipo")]
        //public int Tipo { get; set; } 
    }
}
