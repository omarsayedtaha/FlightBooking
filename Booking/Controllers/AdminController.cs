using System.Net;
using System.Reflection.Metadata.Ecma335;
using Application.Common;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("create-role")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateRole([FromBody] string role)
        {
            var Role = new IdentityRole<Guid>(role);
            var IsRoleExist = await _roleManager.GetRoleNameAsync(Role);
            if (IsRoleExist == null)
            {
                return BadRequest(new BaseResponse<string> { StatusCode = HttpStatusCode.BadRequest, Message = "Role already exist" });
            }
            await _roleManager.CreateAsync(Role);
            return Ok(new BaseResponse<string> { StatusCode = HttpStatusCode.OK, Message = "New Role Created", Data = Role.Name ?? string.Empty });
        }

        [HttpPut("assign-users-to-roles")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssignRoles([FromQuery] Guid userId, [FromQuery] Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (user == null)
                return NotFound(new BaseResponse<string> { StatusCode = HttpStatusCode.NotFound, Message = "User doesn't exist" });

            if (role == null)
                return NotFound(new BaseResponse<string> { StatusCode = HttpStatusCode.NotFound, Message = "Role doesn't exist" });

            await _userManager.AddToRoleAsync(user, role.Name);
            return Ok(new BaseResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"User: {user.UserName} is assigned to Role: {role.Name}  ",
                Data = string.Empty
            });
        }

        [HttpPut("remove-users-from-roles")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveUserFromRole([FromQuery] Guid userId, [FromQuery] Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (user == null)
                return NotFound(new BaseResponse<string> { StatusCode = HttpStatusCode.NotFound, Message = "User doesn't exist" });

            if (role == null)
                return NotFound(new BaseResponse<string> { StatusCode = HttpStatusCode.NotFound, Message = "Role doesn't exist" });

            await _userManager.RemoveFromRoleAsync(user, role.Name);
            return Ok(new BaseResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"User: {user.UserName} is removed from Role: {role.Name}",
                Data = string.Empty
            });
        }
    }
}
