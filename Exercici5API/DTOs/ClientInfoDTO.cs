namespace Exercici5API.DTOs
{
    public class ClientInfoDTO
    {
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string CEOName { get; set; }
        public int NumberOfAttendees { get; set; }
        public bool IsVip { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
