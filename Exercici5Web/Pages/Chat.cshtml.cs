using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercici5Web.Pages
{
    public class ChatModel : PageModel
    {
        public string? ErrorMessage { get; set; }
        public void OnGet()
        {
        }
    }
}
