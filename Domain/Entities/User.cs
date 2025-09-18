using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Token { get; set; }
        //public Guid? RefreshToken { get; set; }
        //public DateTime? RefreshTokenExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PassportNumber { get; set; }
        public string Nationality { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
