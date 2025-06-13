using Exercici5API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace Exercici5Web.Pages
{
    public class EditClientModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        [BindProperty]
        public ClientUpdateDTO Client { get; set; }

        public EditClientModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            var clientApi = _httpClient.CreateClient("ClientApi");

            var response = await clientApi.GetAsync($"api/Auth/{email}");

            if (response.IsSuccessStatusCode)
            {
                Client = await response.Content.ReadFromJsonAsync<ClientUpdateDTO>();
                return Page();
            }
            Console.WriteLine(response.Content);

            return RedirectToPage("/Error");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var clientApi = _httpClient.CreateClient("ClientApi");
            var token = HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await clientApi.PutAsJsonAsync("api/Auth/update", Client);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Index");

            return Page();
        }
    }
}
