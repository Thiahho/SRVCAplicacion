using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    public class Departamento
    {
        [Key]
        public int id_dp { get; set; }
        [MaxLength(45)]
        public string departamento{ get; set; } = "";

    }
}
