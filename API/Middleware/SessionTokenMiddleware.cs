using Lib.models;
using Lib.services;

namespace API.Middleware
{
    /// <summary>
    /// Reads the "Authorization: Bearer &lt;token&gt;" header and restores the
    /// matching <see cref="Session"/> so downstream controllers work correctly.
    ///
    /// The "token" is simply the user's email (base-64 encoded).  This is
    /// intentionally lightweight — swap for JWT if you need production-grade auth.
    /// </summary>
    public class SessionTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionTokenMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

            if (authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
            {
                var token = authHeader["Bearer ".Length..].Trim();
                try
                {
                    var email = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));
                    var user = UserService.SearchUser("email", email);
                    if (user != null)
                        Session.Instance.Account = user;
                    else
                        Session.Instance.Account = null;
                }
                catch
                {
                    Session.Instance.Account = null;
                }
            }
            else
            {
                Session.Instance.Account = null;
            }

            await _next(context);
        }
    }

    /// <summary>Helper that generates and decodes session tokens.</summary>
    public static class TokenHelper
    {
        /// <summary>Encode a user's email into an opaque token.</summary>
        public static string Encode(string email)
            => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(email));
    }
}
