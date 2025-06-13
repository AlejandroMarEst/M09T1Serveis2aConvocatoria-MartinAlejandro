using System.ComponentModel.DataAnnotations;

namespace Exercici5Web.DTOs
{
    public class ClientRegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string CEOName { get; set; }
        [Required]
        public int NumberOfAttendees { get; set; }
        [Required]
        public bool IsVip { get; set; }
    }
}
