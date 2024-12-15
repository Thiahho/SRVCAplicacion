using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Cryptography;
using System.Text;

namespace SRVCAplicacion
{
    public class Programa
    {

        static void Main(string[] args)
        {
            string encriptedFilePath = @"C:\Users\thiag\source\repos\SRVCAplicacion\SRVCAplicacion\wwwroot\encryptedData.dat";

            if (!File.Exists(encriptedFilePath))
            {
                Console.WriteLine("El archivo no existe en la ruta especificada.");
                return;
            }

            try
            {
                byte[] data = File.ReadAllBytes(encriptedFilePath);

                string descriptador = UnProtected(data);

                Console.WriteLine("Contraseña descrifada:" + descriptador);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio un error: " + ex.Message);
            }
        }

        public static string UnProtected(byte[] encriptado)
        {
            byte[] descriptado = ProtectedData.Unprotect(encriptado, null, DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(descriptado);
        }

    }
}
