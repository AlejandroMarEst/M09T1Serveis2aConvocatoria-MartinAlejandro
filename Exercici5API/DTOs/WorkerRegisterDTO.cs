namespace Exercici5API.DTOs
{
    public class ClientRegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string CEOName { get; set; }
        public int NumberOfAttendees { get; set; }
        public bool IsVip { get; set; }
    }
}
