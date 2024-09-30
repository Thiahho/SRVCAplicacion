using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SRVCAplicacion.Data;
using SRVCAplicacion.Models;
using System.Net.Http;

namespace SRVCAplicacion.Controllers
{
    public class HistorialController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly HttpClient _httpClient;
        public HistorialController(ApplicationDbContext appDbContext, HttpClient http)
        {
            _appDbContext = appDbContext;
            _httpClient = http;
        }
        public IActionResult Index()
        {
            try
            {
                var registro = _appDbContext.Registro.OrderByDescending(p => p.Id).ToList();
                return View(registro); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public async Task<List<Registro>> BuscarRegistroAsync(string condicion, DateTime desde, DateTime hasta)
        //{
        //    List<Registro> registros = new List<Registro>();
        //    try
        //    {
        //        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            string sqlQuery = @"
        //        SELECT Identificacion, Nombre, Apellido, Departamento, HoraDeIngreso, HoraDeSalida, Motivo 
        //        FROM registro r
        //        WHERE (@condicion IS NULL OR r.Nombre LIKE @condicion) 
        //        AND r.HoraDeIngreso >= @fechaDesde 
        //        AND r.HoraDeSalida <= @fechaHasta";

        //            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
        //            {
        //                cmd.Parameters.AddWithValue("@condicion", string.IsNullOrEmpty(condicion) ? (object)DBNull.Value : $"%{condicion}%");
        //                cmd.Parameters.AddWithValue("@fechaDesde", desde);
        //                cmd.Parameters.AddWithValue("@fechaHasta", hasta);

        //                await con.OpenAsync();
        //                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await reader.ReadAsync())
        //                    {
        //                        registros.Add(new Registro
        //                        (
        //                            reader["Identificacion"].ToString(),
        //                            reader["Nombre"].ToString(),
        //                            reader["Apellido"].ToString(),
        //                            reader["Departamento"].ToString(),
        //                            Convert.ToDateTime(reader["HoraDeIngreso"]),
        //                            Convert.ToDateTime(reader["HoraDeSalida"]),
        //                            reader["Motivo"].ToString()
        //                        ));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error al buscar registro: " + ex.Message);
        //    }
        //    return registros;
        //}
        public async Task<List<Registro>> BuscarRegistro(string condicion, DateTime desde, DateTime hasta)
        {
            List<Registro> registros = new List<Registro>();

            try
            {
                // Crea la URL de la API
                string apiUrl = $"https://localhost:7285/api/registro?condicion={Uri.EscapeDataString(condicion)}&desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}";
                // Realiza la solicitud a la API
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa

                // Deserializa la respuesta en una lista de registros
                registros = await response.Content.ReadFromJsonAsync<List<Registro>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar registros: " + ex.Message);
            }

            return registros;
        }
    }
}
