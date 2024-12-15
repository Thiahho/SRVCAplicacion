using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SRVCAplicacion;

namespace Manager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly string _encryptedFilePath = @"C:\Users\thiag\source\repos\Manager\Manager\wwwroot\encryptedData.dat";

        public string DecryptedPassword {  get; set; }    


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                DecryptedPassword = CryptoFiles.GetDesencriptadorPassword(_encryptedFilePath);

            }
            catch (Exception ex)
            {
                DecryptedPassword = $"Error al descriptar :{ex.Message}";
            }
        }
    }
}
