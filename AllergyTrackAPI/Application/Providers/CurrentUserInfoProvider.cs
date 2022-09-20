using Application.Providers.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Providers
{
    public class CurrentUserInfoProvider : ICurrentUserInfoProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Guid GetUserInfo()
        {
            var userGuid = new Guid(_httpContextAccessor?.HttpContext?.User?.Claims?
                                                         .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            return userGuid;
        }
    }
}
