using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SRVCAplicacion.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ApplicationDbContext _context;

        public ApplicationDbContext(DbContextOptions options, ApplicationDbContext context) : base(options)
        {
            _context = context;

        }

        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Entra> Entra { get; set; }
        public DbSet<Motivo> Motivo { get; set; }
        public DbSet<registro_visitas> registro_Visitas { get; set; }
        public DbSet<Salida> Salida { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<visitante_inquilino> visitante_Inquilino { get; set; }
        public DbSet<log_aud> log_Aud { get; set; }
        public DbSet<Puntos_de_controles> Puntos_de_controles { get; set; }


        public int GetUserId(int id)
        {
            var usuario = _context.Usuario.FirstOrDefaultAsync(us => us.id_usuario == id);
            return usuario?.
        }
        public override async Task<int> SaveChangesAasync(CancellationToken cancellation = default)
        {
            var cambios= ChangeTracker.Entries().Where(e=>e.State==EntityState.Modified)
                .ToList();

            foreach(var cambio in cambios)
            {
                var originalValores= cambio.OriginalValues;
                var cambioValores = cambio.CurrentValues;

                foreach(var propiedad in originalValor.Properties)
                {
                    var orgValor= originalValores[propiedad]?.ToString();
                    var modValor= cambioValores[propiedad]?.ToString();

                    if (orgValor != modValor) 
                    {
                        var log = new LogAud
                        {
                            id_usuario = GetUserId(),
                            accion= $"Modificacion en  {cambio.Entity.GetType().Name}",
                            hora=DateTime.UtcNow,
                            valor_original=orgValor,
                            valor_nuevo= modValor,
                            id_punto_control=
                    }
                }
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;Database=db;User=root;Password=123456;",
                    new MySqlServerVersion(new Version(8, 0, 21)),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure());
            }
        }

       
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
