namespace Examplium.Server.Services.Auth
{
    public interface IAuthService
    {
        string? GetUserId();
        string? GetUserEmail();
    }
}
