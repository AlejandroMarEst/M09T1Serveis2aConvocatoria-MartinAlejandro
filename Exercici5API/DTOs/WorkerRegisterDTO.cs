using System.ComponentModel.DataAnnotations;

namespace Exercici5API.DTOs
{
    public class WorkerRegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
