namespace WalletService.API.Services;

using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using WalletService.Application.Interfaces;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
            return;

        var nameIdentifier = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues result))
        {
            if (result.Count > 0)
            {
                AuthenticationBearer = result[0].Replace("Bearer ", "");
            }
        }

        if (string.IsNullOrEmpty(nameIdentifier))
        {
            return;
        }

        UserId = nameIdentifier;

    }

    public string UserId { get; }
    public string AuthenticationBearer { get; }
}