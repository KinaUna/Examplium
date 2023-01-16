using Duende.IdentityServer.Extensions;

namespace Examplium.Server.Services.Auth
{
    public class AuthService: IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId() => _httpContextAccessor.HttpContext?.User.Identity.GetSubjectId();

        public string? GetUserEmail() => _httpContextAccessor.HttpContext?.User.FindFirst("email")?.Value;
    }
}
