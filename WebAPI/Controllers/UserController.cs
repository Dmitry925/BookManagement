using BookManagement.Core.Application.Authentication;
using BookManagement.Core.Application.Features.UserFeatures.Commands;
using BookManagement.Core.Application.Features.UserFeatures.Queries;
using BookManagement.Core.Application.Interfaces;
using BookManagement.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookManagement.Presentation.WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Authorize(Roles = nameof(BaseRole.Admin))]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery.Request request)
        {
            return Ok(await SendAsync(request));
        }

        [HttpPost("token")]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationModel model)
        {
            return Ok(await _userService.GetTokenAsync(model));
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpUserCommand.Request request)
        {
            return Ok(await SendAsync(request));
        }
    }
}
