using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Models
{
    public class Motivo
    {
        protected int Id { get; set; } = 0;
        [MaxLength(150)]
        protected string Descripcion { get; set; } = "";

    }
}
