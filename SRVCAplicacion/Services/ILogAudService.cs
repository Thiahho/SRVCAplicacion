using SRVCAplicacion.Data;
using SRVCAplicacion.Models;

namespace SRVCAplicacion.Services
{
    public interface ILogAudService
    {
        Task RegistrarCambio<T>(
            string tabla,
            string accion,
            T valor_ori,
            T valor_nuevo,
            int id_user,
            int id_punto,
            DateTime horaLog
        ) where T : class;
    }

    public class AuditoriaSerice : ILogAudService
    {
        private readonly ApplicationDbContext _context;

        public AuditoriaSerice(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarCambio<T>(string tabla, string accion, T valor_ori, T valor_nuevo,int id_user, int id_punto, DateTime horaLog) where T : class 
        {
            var propiedades = typeof(T).GetProperties();

            foreach(var propiedad in propiedades)
            {
                var valorOri= propiedad.GetValue(valor_ori)?.ToString()?? string.Empty;
                var valorNuevoStr= propiedad.GetValue(valor_nuevo)?.ToString()?? string.Empty;
            
                if(valorOri != valorNuevoStr)
                {
                    var auditoria = new log_aud
                    {
                        id_usuario = id_user,
                        id_log_aud = id_punto,
                        accion = accion,
                        valor_original = valorOri,
                        valor_nuevo = valorNuevoStr,
                        hora = horaLog
                    };
                    _context.log_Aud.Add(auditoria);

                }
            }

            await _context.SaveChangesAsync();
        }


    }
}
