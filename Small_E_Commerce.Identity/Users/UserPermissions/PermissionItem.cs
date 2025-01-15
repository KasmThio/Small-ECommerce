namespace Small_E_Commerce.Identity.Users.UserPermissions;

public sealed class PermissionItem
{
    public int Id { get; private set; }

    public PermissionItem(int id)
    {
        Id = id;
    }
}