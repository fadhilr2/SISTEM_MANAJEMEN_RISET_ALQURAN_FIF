using API.Dtos;
using Lib.services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    [Tags("Users")]
    public class UserController : ControllerBase
    {
        // ── GET /api/users ───────────────────────────────────────────────────

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        public IActionResult GetAll()
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("Admin access required."));

            var users = DataContext.Users.GetAll()
                .Select(u => new UserDto(u.Name ?? string.Empty, u.Email, u.Role));

            return Ok(ApiResponse.Ok(users));
        }

        // ── GET /api/users/search ────────────────────────────────────────────

        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult Search([FromQuery] string field, [FromQuery] string value)
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("Admin access required."));

            if (field != "email" && field != "name")
                return BadRequest(ApiResponse.Fail("Field must be 'email' or 'name'."));

            var user = UserService.SearchUser(field, value);
            if (user == null)
                return NotFound(ApiResponse.Fail($"User with {field} '{value}' not found."));

            return Ok(ApiResponse.Ok(new UserDto(user.Name ?? string.Empty, user.Email, user.Role)));
        }

        // ── GET /api/users/profile ───────────────────────────────────────────

        /// <summary>Get the authenticated user's own profile.</summary>
        /// <response code="200">Profile of the current user.</response>
        /// <response code="401">Not authenticated.</response>
        [HttpGet("profile")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var account = Session.Instance.Account;
            if (account == null)
                return Unauthorized(ApiResponse.Fail("Authentication required."));

            return Ok(ApiResponse.Ok(
                new UserDto(account.Name ?? string.Empty, account.Email, account.Role)));
        }

        // ── PATCH /api/users/profile ─────────────────────────────────────────

        /// <summary>Edit the authenticated user's name or email.</summary>
        /// <remarks>
        /// **Sample request — change name:**
        /// ```json
        /// { "field": "name", "value": "New Name" }
        /// ```
        /// **Sample request — change email:**
        /// ```json
        /// { "field": "email", "value": "new@example.com" }
        /// ```
        /// </remarks>
        /// <response code="200">Profile updated successfully.</response>
        /// <response code="400">Invalid field. Must be 'name' or 'email'.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="404">Current user account not found in data store.</response>
        [HttpPatch("profile")]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult EditProfile([FromBody] EditProfileRequest body)
        {
            var account = Session.Instance.Account;
            if (account == null)
                return Unauthorized(ApiResponse.Fail("Authentication required."));

            switch (body.Field.ToLowerInvariant())
            {
                case "name":
                {
                    var user = UserService.SearchUser("email", account.Email);
                    if (user == null)
                        return NotFound(ApiResponse.Fail("Current user not found."));

                    string oldName = user.Name ?? string.Empty;
                    user.Name = body.Value;
                    account.Name = body.Value;

                    // propagate name change to papers
                    if (!oldName.Equals(body.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var paper in DataContext.Papers.GetAll())
                        {
                            if (paper.Author.Equals(oldName, StringComparison.OrdinalIgnoreCase))
                                paper.Author = body.Value;
                        }
                    }
                    return Ok(ApiResponse.Ok("Name updated successfully."));
                }

                case "email":
                {
                    var user = UserService.SearchUser("email", account.Email);
                    if (user == null)
                        return NotFound(ApiResponse.Fail("Current user not found."));

                    user.Email = body.Value;
                    account.Email = body.Value;
                    return Ok(ApiResponse.Ok("Email updated successfully."));
                }

                default:
                    return BadRequest(ApiResponse.Fail("Field must be 'name' or 'email'."));
            }
        }

        // ── POST /api/users/role ─────────────────────────────────────────────

        /// <summary>Toggle a user's role between visitor and researcher (admin only).</summary>
        /// <remarks>
        /// **Sample request:**
        /// ```json
        /// { "name": "Muhammad Rasul" }
        /// ```
        /// </remarks>
        /// <response code="200">Role toggled successfully.</response>
        /// <response code="403">Admin access required.</response>
        /// <response code="404">User not found.</response>
        [HttpPost("role")]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult ChangeRole([FromBody] ChangeRoleRequest body)
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("Admin access required."));

            var user = UserService.SearchUser("name", body.Name);
            if (user == null)
                return NotFound(ApiResponse.Fail($"User '{body.Name}' not found."));

            string oldRole = user.Role;
            if (user.Role.Equals("visitor", StringComparison.OrdinalIgnoreCase))
                user.Role = "researcher";
            else if (user.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase))
                user.Role = "visitor";

            return Ok(ApiResponse.Ok($"Role changed from '{oldRole}' to '{user.Role}'."));
        }

        // ── GET /api/users/requests ──────────────────────────────────────────

        /// <summary>List all pending registration requests (admin only).</summary>
        /// <response code="200">Array of pending registration requests.</response>
        /// <response code="403">Admin access required.</response>
        [HttpGet("requests")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        public IActionResult GetRequests()
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("Admin access required."));

            var requests = DataContext.Requests.GetAll()
                .Select(u => new UserDto(u.Name ?? string.Empty, u.Email, u.Role));

            return Ok(ApiResponse.Ok(requests));
        }

        // ── POST /api/users/requests/accept ─────────────────────────────────

        /// <summary>Accept a pending registration request (admin only).</summary>
        /// <remarks>
        /// Moves the user from the pending queue into the active user store.
        ///
        /// **Sample request:**
        /// ```json
        /// { "email": "bacon@mail.com" }
        /// ```
        /// </remarks>
        /// <response code="200">Request accepted; user is now active.</response>
        /// <response code="403">Admin access required.</response>
        /// <response code="404">No pending request with that email.</response>
        [HttpPost("requests/accept")]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
        public IActionResult AcceptRequest([FromBody] AcceptRequestBody body)
        {
            if (!IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("Admin access required."));

            var requester = DataContext.Requests.GetAll()
                .FirstOrDefault(u => u.Email.Equals(body.Email, StringComparison.OrdinalIgnoreCase));

            if (requester == null)
                return NotFound(ApiResponse.Fail($"No pending request for email '{body.Email}'."));

            DataContext.Users.Add(requester);
            DataContext.Requests.Remove(requester);

            return Ok(ApiResponse.Ok($"Request for '{body.Email}' accepted. User is now active."));
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static bool IsAdmin()
            => Session.Instance.Account?.Role.Equals("admin", StringComparison.OrdinalIgnoreCase) == true;
    }
}
