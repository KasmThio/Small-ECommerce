using Microsoft.AspNetCore.Authorization;

namespace Small_E_Commerce.WebApi.Authorization;

internal sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; private set; }
}