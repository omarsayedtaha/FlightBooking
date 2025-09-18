using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string? Token { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? RefreshToken { get; set; }
    }
}
