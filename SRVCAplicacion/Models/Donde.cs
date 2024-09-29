using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    public class Donde
    {
        [Key][Column("id")]
        public int Id { get; set; }
        [MaxLength(80)]
        public string Descripcion { get; set; } = "";

    }
}
