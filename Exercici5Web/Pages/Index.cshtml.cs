using Exercici5Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace Exercici5Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public List<ClientInfoDTO> ClientList { get; set; } = new List<ClientInfoDTO>();

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("ClientApi");
        try
        {
            var response = await client.GetAsync("api/Auth/clients");
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogError("Error loading games");
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                ClientList = JsonSerializer.Deserialize<List<ClientInfoDTO>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
