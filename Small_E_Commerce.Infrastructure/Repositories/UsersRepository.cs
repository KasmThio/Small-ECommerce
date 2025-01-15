using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Identity;
using Small_E_Commerce.Identity.Users;

namespace Small_E_Commerce.Infrastructure.Repositories;

public class UsersRepository(IdentityDbContext identityDbContext) : IUsersRepository
{
    public async ValueTask<User?> GetAggregateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await identityDbContext
            .Set<User>()
            .Include(x => x.Permissions)
            .Include(x => x.Role)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task NewAsync(User user, CancellationToken cancellationToken)
    {
        await identityDbContext
            .Set<User>()
            .AddAsync(user, cancellationToken);
        await identityDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> HasPermission(Guid id, int permissionId, CancellationToken cancellationToken)
    {
        return await identityDbContext.Set<User>().AnyAsync(x =>
            x.Id == id && x.Permissions.Any(x => x.PermissionId == permissionId), cancellationToken: cancellationToken);
    }

    public async Task<bool> HasRole(Guid id, string role, CancellationToken cancellationToken)
    {
        return await identityDbContext.Set<User>().AnyAsync(x => x.Id == id && x.Role.Name == role, cancellationToken: cancellationToken);
    }
}