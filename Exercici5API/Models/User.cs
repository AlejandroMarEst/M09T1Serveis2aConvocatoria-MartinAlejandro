using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Exercici5API.Models
{
    public class User : IdentityUser
    {
        public string? CompanyName { get; set; }
        public string? CEOName { get; set; }
        public int? NumberOfAttendees { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public bool? IsVip { get; set; }
    }
}
