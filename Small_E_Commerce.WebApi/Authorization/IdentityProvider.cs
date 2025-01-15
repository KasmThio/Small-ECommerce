using System.Security.Claims;
using System.Text.Json;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.WebApi.Authentication;

public class IdentityProvider(
    IHttpContextAccessor httpContextAccessor)
    : IIdentityProvider
{
    
    private Guid _currentUserId;
    public Guid? CurrentUser
    {
        get
        {

            if (!IsAuthenticated)
            {
                return null;
            }

            if (_currentUserId == default)
            {
                _currentUserId = Guid.Parse(
                    httpContextAccessor.HttpContext!.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)!.Value);
            }

            return _currentUserId;
        }
    }
    public string? Email { get; }
    
    
    private bool _isFromHttpContextSet;
    private bool _isFromHttpContext;
    
    private bool _isAuthenticatedSet;
    private bool _isAuthenticated;

    public bool IsAuthenticated
    {
        get
        {
            if (_isAuthenticatedSet)
            {
                return _isAuthenticated;
            }
            _isAuthenticated = httpContextAccessor.HttpContext?.User.Identities.FirstOrDefault()?.IsAuthenticated is true;
            _isAuthenticatedSet = true;
            return _isAuthenticated;
        }
    }
}
