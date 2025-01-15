using Dawn;
using Microsoft.AspNetCore.Identity;
using Small_E_Commerce.Identity.Users.UserPermissions;

namespace Small_E_Commerce.Identity.Users;

public class User : IdentityUser<Guid>
{
    public bool IsAdmin { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    private readonly List<UserPermission> _permissions = new();
    public IEnumerable<UserPermission> Permissions => _permissions;
    public UserRole? Role { get; private set; }
    
    private User(){}
    
    public User(Email email, DateTimeOffset at, bool isAdmin = false)
    {
        Email = email.EmailAddress;
        NormalizedEmail = email.EmailAddress.ToLower();
        UserName = email.EmailAddress.ToUpper();
        IsAdmin = isAdmin;
        CreatedAt = at;
    }
    
    public void GrantPermissions(IEnumerable<UserPermission> permissions)
    {
        Guard.Argument(permissions, nameof(permissions)).NotNull();
        _permissions.Clear();
        foreach (var permission in permissions)
        {
            _permissions.Add(new UserPermission(permission.PermissionId));
        }
    }
    
    
    public void GrantRole(UserRole role)
    {
        Role = Guard.Argument(role, nameof(role)).NotNull();
        IsAdmin = role.Name != Roles.Customer;
    }
}