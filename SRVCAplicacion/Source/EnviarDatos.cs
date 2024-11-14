using Newtonsoft.Json;
using SRVCAplicacion.Models;
using System.Text;

namespace SRVCAplicacion.Source
{
    public class EnviarDatos
    {
        public async Task EnviarDatosAlServidor(IEnumerable<Usuario> usuarios, IEnumerable<log_aud> logAud, IEnumerable<visitante_inquilino> visitantesInquilinos)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://mi-servidor.com/api/actualizar_punto");

                var contentUsuarios = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");
                var contentLogAud = new StringContent(JsonConvert.SerializeObject(logAud), Encoding.UTF8, "application/json");
                var contentVisitantes = new StringContent(JsonConvert.SerializeObject(visitantesInquilinos), Encoding.UTF8, "application/json");

                var responseUsuarios = await client.PostAsync("api/usuarios/obtener", contentUsuarios);
                var responseLogAud = await client.PostAsync("api/logaud/logaud", contentLogAud);
                var responseVisitantes = await client.PostAsync("api/inquilino/ObtenerTodos", contentVisitantes);

                if (responseUsuarios.IsSuccessStatusCode && responseLogAud.IsSuccessStatusCode && responseVisitantes.IsSuccessStatusCode)
                {
                    Console.WriteLine("Datos enviados exitosamente");
                }
                else
                {
                    Console.WriteLine("Error al enviar los datos");
                }
            }
        }
    }
}
