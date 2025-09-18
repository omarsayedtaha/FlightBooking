using System.Text.Json.Serialization;

namespace Application.Dtos.User
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportNumber { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
    }
}
