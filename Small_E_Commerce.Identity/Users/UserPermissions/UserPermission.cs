using Dawn;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Identity.Users.UserPermissions;

public class UserPermission : EntityBase<Guid>
{
    public Guid UserId { get; private set; }
    public int PermissionId { get; private set; }

    public UserPermission(int permissionId)
    {
        PermissionId = Guard.Argument(permissionId, nameof(permissionId)).GreaterThan(0);
    }
}