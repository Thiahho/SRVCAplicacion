using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRVCAplicacion.Models
{
    [Table("registro_visitas")]

    public class registro_visitas
    {
        [Key]
        public int id_registro_visitas { get; set; } = 0;  // ID del registro de la visita

        public int? id_usuario { get; set; } = 0;  // ID del usuario que registra la visita

        [MaxLength(45)]
        public string? nombre_encargado { get; set; } = "";  // Nombre del encargado

        [MaxLength(45)]
        public string motivo { get; set; } = "";  // Motivo de la visita

        [MaxLength(45)]
        public string motivo_personalizado { get; set; } = "";  // Motivo personalizado de la visita

        [MaxLength(45)]
        public string depto_visita { get; set; } = "";  // Departamento donde se realiza la visita

        public DateTime? hora_ingreso { get; set; }  // Hora de ingreso de la visita

        public DateTime? hora_salida { get; set; }  // Hora de salida de la visita

        public int id_visitante_inquilino { get; set; } = 0;  // ID del visitante o inquilino

        [MaxLength(45)]
        public string nombre_visitante_inquilino { get; set; } = "";  // Nombre del visitante o inquilino

        [MaxLength(45)]
        public string identificacion_visita { get; set; } = "";  // Identificación de la visita

        public int estado_visita { get; set; }  // Estado de la visita (dentro o fuera)

        public int id_punto_control { get; set; } = 0;  // ID del punto de control asociado

        [MaxLength(45)]
        public string nombre_punto_control { get; set; } = "";
        //public Registro(string identificacion, string nombre, string apellido, string departamento, DateTime horaDeIngreso, DateTime horaDeSalida, string motivo)
        //{
        //    Identificacion = identificacion;
        //    Nombre = nombre;
        //    Apellido = apellido;
        //    Departamento = departamento;
        //    HoraDeIngreso = horaDeIngreso;
        //    HoraDeSalida = horaDeSalida;
        //    Motivo = motivo;
        //}
    }
}
