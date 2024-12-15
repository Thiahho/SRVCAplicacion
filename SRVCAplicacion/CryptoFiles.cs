using System.Security.Cryptography;
using System.Text;

namespace SRVCAplicacion
{
    public class CryptoFiles
    {
        public static string UnprotectData(byte[] encryptedData)
        {
            // Descifra los datos con DPAPI usando el contexto del usuario actual
            byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decryptedData);
        }

        public static string GetDesencriptadorPassword(string filePath)
        {
            byte[] encriptador= File.ReadAllBytes(filePath);

            return UnprotectData(encriptador);
        }


    }
}
