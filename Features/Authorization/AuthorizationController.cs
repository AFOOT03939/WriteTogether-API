using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Linq;
using WriteTogether.Features.Authorization;

namespace WriteTogether.Features.Authorization
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : ControllerBase
    {
        public readonly AuthorizationService _authService;
        public readonly AuthorizationRepository _authRepo;
        public AuthorizationController(AuthorizationService authService, AuthorizationRepository authRepo)
        {
            _authService = authService;
            _authRepo = authRepo;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AuthorizationModel request)
        {
            if (string.IsNullOrEmpty(request.Email)) return BadRequest("Email error");

            if (string.IsNullOrEmpty(request.Password)) return BadRequest("Password error");

            var token = await _authService.Login(request);

            var name = await _authService.FetchUsers(request);

            return Ok(new {token, name});
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var usernameClaim = User.FindFirst(ClaimTypes.Name);

            if (userIdClaim == null || usernameClaim == null)
                return Unauthorized("Invalid token");

            return Ok(new
            {
                userId = int.Parse(userIdClaim.Value),
                userName = usernameClaim.Value
            });
        }
    }
}
