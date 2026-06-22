namespace API.Dtos
{
    public record LoginRequest(string Email, string Password);

    public record RegisterRequest(string Name, string Email, string Password);

    public record EditProfileRequest(string Field, string Value);

    public record ChangeRoleRequest(string Name);

    public record AcceptRequestBody(string Email);

    public record UploadPaperRequest(string Title, string Abstract);

    public record EditTitleRequest(string CurrentTitle, string NewTitle);

    public record EditAbstractRequest(string Title, string NewAbstract);

    public record DeletePaperRequest(string Title);
}
