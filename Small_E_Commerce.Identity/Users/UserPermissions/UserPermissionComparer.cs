using System.Diagnostics.CodeAnalysis;

namespace Small_E_Commerce.Identity.Users.UserPermissions;

internal class UserPermissionComparer : IEqualityComparer<UserPermission>
{
    public static readonly UserPermissionComparer Instance = new UserPermissionComparer();

    public bool Equals(UserPermission? x, UserPermission? y)
    {
        if (x is not null && y is not null)
            return x.PermissionId.Equals(y.PermissionId);

        return object.Equals(x, y);
    }

    public int GetHashCode([DisallowNull] UserPermission obj)
    {
        return obj.PermissionId.GetHashCode();
    }
}