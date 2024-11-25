using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SRVCAplicacion.Models;
using System.ComponentModel.DataAnnotations;

namespace SRVCAplicacion.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
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
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Entra> Entra { get; set; }
        public DbSet<Motivo> Motivo { get; set; }
        public DbSet<registro_visitas> registro_Visitas { get; set; }
        public DbSet<Salida> Salida { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<visitante_inquilino> visitante_Inquilino { get; set; }
        public DbSet<log_aud> log_Aud { get; set; }
        public DbSet<Puntos_de_controles> Puntos_de_controles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(d => d.id_dp);
            });
            modelBuilder.Entity<Usuario>().HasOne<Puntos_de_controles>().WithMany().HasForeignKey(u => u.id_punto_control);
            modelBuilder.Entity<registro_visitas>().HasKey(r => r.id_usuario);
            modelBuilder.Entity<Motivo>().HasKey(r => r.id_motivo);
            modelBuilder.Entity<Puntos_de_controles>();

        }
        private Usuario ObtenerUsuarioActual()
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst("id_usuario")?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                // Buscar el usuario en la base de datos
                return Usuario.FirstOrDefault(u => u.id_usuario == userId);
            }

            return null; // Usuario no encontrado o no autenticado
        }

        protected void DetectarRegistroCambios()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entityName = entry.Entity.GetType().Name;
                var entityId = entry.Properties
                                .FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue;

                var userActual = ObtenerUsuarioActual();
                if (userActual == null)
                {
                    throw new Exception("No se pudo obtener el usuario actual.");
                }

                foreach (var property in entry.OriginalValues.Properties)
                {
                    var original= entry.OriginalValues[property]?.ToString();
                    var nuevo= entry.OriginalValues[property]?.ToString();
                    
                    if(original != nuevo)
                    {
                        var log = new log_aud
                        {
                            id_usuario = userActual.id_usuario,
                            valor_original = original,
                            valor_nuevo = nuevo,
                            hora = DateTime.UtcNow,
                            id_punto_control = userActual.id_punto_control,
                            accion = $"Modificacion en {entityName} (ID:{entityId})"
                        };
                        log_Aud.Add(log);
                    }
                }
            }
        }


        public override int SaveChanges()
        {
            DetectarRegistroCambios();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DetectarRegistroCambios();
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
