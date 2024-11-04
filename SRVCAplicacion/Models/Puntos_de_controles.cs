using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    [Table("puntos_de_controles")]
    public class Puntos_de_controles
    {
        [Key]
        public int id_punto_control { get; set; }

        public string token { get; set; }
        public string nombre_punto_control{  get; set; }
        public int estado {  get; set; }
        public string id{  get; set; }
    }
}
