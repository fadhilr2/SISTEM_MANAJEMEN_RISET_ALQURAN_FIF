namespace API.Dtos
{
    // ── Auth ──────────────────────────────────────────────────────────────────

    /// <summary>Credentials for logging in.</summary>
    public record LoginRequest(string Email, string Password);

    /// <summary>Payload for registering a new account.</summary>
    public record RegisterRequest(string Name, string Email, string Password);

    // ── User ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Request body for editing the authenticated user's profile field.
    /// <br/><b>Field:</b> "name" or "email" — <b>Value:</b> new value to assign.
    /// </summary>
    public record EditProfileRequest(string Field, string Value);

    /// <summary>Request body for toggling a user's role (admin only). Name = target user's display name.</summary>
    public record ChangeRoleRequest(string Name);

    /// <summary>Request body for accepting a registration request (admin only). Email = pending request email.</summary>
    public record AcceptRequestBody(string Email);

    // ── Paper ─────────────────────────────────────────────────────────────────

    /// <summary>Payload for uploading a new paper. Title = paper title; Abstract = abstract text.</summary>
    public record UploadPaperRequest(string Title, string Abstract);

    /// <summary>
    /// Payload for editing a paper's title.
    /// <br/><b>CurrentTitle:</b> current title used to locate the paper.
    /// <br/><b>NewTitle:</b> replacement title.
    /// </summary>
    public record EditTitleRequest(string CurrentTitle, string NewTitle);

    /// <summary>
    /// Payload for editing a paper's abstract.
    /// <br/><b>Title:</b> title used to locate the paper.
    /// <br/><b>NewAbstract:</b> replacement abstract text.
    /// </summary>
    public record EditAbstractRequest(string Title, string NewAbstract);

    /// <summary>Payload for deleting a paper. Title = exact title of the paper to delete.</summary>
    public record DeletePaperRequest(string Title);
}
