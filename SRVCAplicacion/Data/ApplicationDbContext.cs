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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=db;Username=postgres;Password=123456");
            }
        }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Motivo> Motivo { get; set; }
        public DbSet<registro_visitas> registro_Visitas { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<visitante_inquilino> visitante_Inquilino{ get; set; }
        public DbSet<log_aud> log_Aud{ get; set; }
        public DbSet<Puntos_de_controles> Puntos_de_controles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(d => d.id_dp);
            });
            modelBuilder.Entity<Usuario>().HasOne<Puntos_de_controles>().WithMany().HasForeignKey(u=>u.id_punto_control);
            modelBuilder.Entity<registro_visitas>().HasKey(r => r.id_usuario);
            modelBuilder.Entity<Motivo>().HasKey(r => r.id_motivo);
            modelBuilder.Entity<Puntos_de_controles>();

        }
    }
}
