using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Exercici5Web.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<UserRegisterModel> _logger;
        public DeleteModel(IHttpClientFactory httpClient, ILogger<UserRegisterModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IActionResult> OnPostAsync(string email)
        {
            var client = _httpClient.CreateClient("ClientApi");
            var token = HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var response = await client.DeleteAsync($"api/Auth/delete/{email}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    _logger.LogInformation("Delete failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToPage("/Index");
        }
    }
}
