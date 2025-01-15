using Small_E_Commerce.Identity.Users.UserPermissions;

namespace Small_E_Commerce.Application;

public record AdminAuthenticationResponse(
    string AccessToken,
    string RefreshToken,
    string Role,
    Guid UserId,
    string UserEmail,
    IEnumerable<Permission> Permissions
);