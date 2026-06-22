namespace API.Dtos
{
    // ── Generic ───────────────────────────────────────────────────────────────

    /// <summary>Standard API envelope returned for all responses.</summary>
    public record ApiResponse<T>(bool Success, string? Message, T? Data);

    public static class ApiResponse
    {
        public static ApiResponse<T> Ok<T>(T data, string? message = null)
            => new(true, message, data);

        public static ApiResponse<object?> Fail(string message)
            => new(false, message, null);

        public static ApiResponse<object?> Ok(string? message = null)
            => new(true, message, null);
    }

    // ── Auth ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Session token returned after a successful login.
    /// Token = opaque bearer token; Name = logged-in user's display name; Role = visitor | researcher | admin.
    /// </summary>
    public record LoginResponse(string Token, string Name, string Role);

    // ── User ──────────────────────────────────────────────────────────────────

    /// <summary>Public representation of a user account. Name, Email, Role.</summary>
    public record UserDto(string Name, string Email, string Role);

    // ── Paper ─────────────────────────────────────────────────────────────────

    /// <summary>Public representation of a research paper. Title, Abstract, Author, Date.</summary>
    public record PaperDto(string Title, string Abstract, string Author, string Date);
}
