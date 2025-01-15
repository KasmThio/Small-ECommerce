using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Small_E_Commerce.Identity.Users;
using Small_E_Commerce.Identity.Users.UserPermissions;

namespace Small_E_Commerce.WebApi.Authorization;

internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory, ILogger<PermissionAuthorizationHandler> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var usersRepository = scope.ServiceProvider.GetRequiredService<IUsersRepository>();

        var userSub = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userSub == null)
        {
            _logger.LogWarning("User sub not found");
            context.Fail();
            return;
        }

        var user = await usersRepository.GetAggregateAsync(Guid.Parse(userSub));
        if (user is null)
        {
            _logger.LogWarning($"User {userSub} not found");
            context.Fail();
            return;
        }

        var permissionId = Permission.All.FirstOrDefault(x => x.Name == requirement.Permission).Id;
        var allPermissionId = Permission.All.FirstOrDefault(x => x.Name == "All").Id;

        if (permissionId == 0)
        {
            _logger.LogWarning($"Permission {requirement.Permission} not found");
            context.Fail();
            return;
        }

        var hasPermission = user.Permissions.Any(x => x.PermissionId == permissionId || x.PermissionId == allPermissionId);

        if (!hasPermission)
        {
            _logger.LogWarning($"User {userSub} does not have permission {requirement.Permission}");
            context.Fail();
            return;
        }

        _logger.LogInformation($"User {userSub} has permission {requirement.Permission}");

        context.Succeed(requirement);
    }
}