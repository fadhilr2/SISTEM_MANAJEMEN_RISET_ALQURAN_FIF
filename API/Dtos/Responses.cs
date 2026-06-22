namespace API.Dtos
{
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

    public record LoginResponse(string Token, string Name, string Role);

    public record UserDto(string Name, string Email, string Role);

    public record PaperDto(string Title, string Abstract, string Author, string Date);
}
