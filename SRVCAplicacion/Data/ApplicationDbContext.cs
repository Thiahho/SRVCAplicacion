using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Models;
using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Data
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Donde>(entity =>
            {
                entity.HasKey(d => d.Id);
            });
            modelBuilder.Entity<Usuario>().HasKey(u => u.id);
            modelBuilder.Entity<Registro>().HasKey(r => r.registro);
        }
    }
}
