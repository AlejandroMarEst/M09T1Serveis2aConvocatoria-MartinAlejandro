using Exercici5Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exercici5Web.Pages
{
    public class UserLoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger _logger;
        [BindProperty]
        public UserLoginDTO Login { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public UserLoginModel(IHttpClientFactory httpClient, ILogger<UserLoginModel> logging)
        {
            _httpClient = httpClient;
            _logger = logging;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            try
            {
                var client = _httpClient.CreateClient("ClientApi");
                var response = await client.PostAsJsonAsync("api/Auth/login", Login);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(token))
                    {
                        HttpContext.Session.SetString("AuthToken", token);
                        _logger.LogInformation("Login susccesfull");
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(token);
                        var roles = jwtToken.Claims
                                            .Where(c => c.Type == ClaimTypes.Role)
                                            .Select(c => c.Value)
                                            .ToList();

                        if (roles.Contains("Client"))
                        {
                            return RedirectToPage("/Profile");
                        }
                        else
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("Login failed");
                    ErrorMessage = "Credencials incorrectes o accés no autoritzat.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durant el login");
                ErrorMessage = "Error inesperat. Torna-ho a intentar.";
            }
            return Page();
        }
    }
}
