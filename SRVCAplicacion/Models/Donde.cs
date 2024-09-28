using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Donde
    {
        protected int Id { get; set; }
        [MaxLength(80)]
        protected string Descripcion { get; set; } = "";

    }
}
