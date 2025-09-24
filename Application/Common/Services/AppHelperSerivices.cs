using System.Security.Claims;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.services
{
    public class AppHelperSerivices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public AppHelperSerivices(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<User> GetUserAsync()
        {
            var useremail = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (useremail == null)
            {
                throw new Exception("Invalid Credentials");

            }
            else
            {
                var user = await _userManager.FindByEmailAsync(useremail);
                if (user == null)
                {
                    throw new Exception("Invalid Credentials");
                }
                return user;
            }
        }
    }
}
