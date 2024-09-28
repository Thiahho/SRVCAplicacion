using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Donde> Donde { get; set; }
        public DbSet<Entra> Entra { get; set; }
        public DbSet<Motivo> Motivo { get; set; }
        public DbSet<Registro> Registro { get; set; }
        public DbSet<Salida> Salida { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
