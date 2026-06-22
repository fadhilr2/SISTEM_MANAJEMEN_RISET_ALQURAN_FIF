using API.Dtos;
using API.Middleware;
using Lib.services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [Tags("Auth")]
    public class AuthController : ControllerBase
    {
        // ── POST /api/auth/login ─────────────────────────────────────────────
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest body)
        {
            bool ok = AuthService.Login(body.Email, body.Password);
            if (!ok)
                return Unauthorized(ApiResponse.Fail("Invalid email or password."));

            var account = Session.Instance.Account!;
            var token = TokenHelper.Encode(account.Email);

            return Ok(ApiResponse.Ok(
                new LoginResponse(token, account.Name ?? string.Empty, account.Role),
                "Login successful."));
        }

        // ── POST /api/auth/register ──────────────────────────────────────────

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status201Created)]
        public IActionResult Register([FromBody] RegisterRequest body)
        {
            AuthService.Register(body.Name, body.Email, body.Password);
            return StatusCode(StatusCodes.Status201Created,
                ApiResponse.Ok("Registration request submitted. Await admin approval."));
        }

        // ── POST /api/auth/logout ────────────────────────────────────────────


        [HttpPost("logout")]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            Session.Instance.Logout();
            return Ok(ApiResponse.Ok("Logged out successfully."));
        }

        // ── GET /api/auth/me ─────────────────────────────────────────────────

        [HttpGet("me")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        public IActionResult Me()
        {
            var account = Session.Instance.Account;
            if (account == null)
                return Unauthorized(ApiResponse.Fail("Not authenticated."));

            return Ok(ApiResponse.Ok(
                new UserDto(account.Name ?? string.Empty, account.Email, account.Role)));
        }
    }
}
