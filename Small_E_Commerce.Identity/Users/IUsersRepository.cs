namespace Small_E_Commerce.Identity.Users;

public interface IUsersRepository
{
    ValueTask<User?> GetAggregateAsync(Guid id, CancellationToken cancellationToken = default);
    Task NewAsync(User user, CancellationToken cancellationToken);
    Task<bool> HasPermission(Guid id, int permissionId, CancellationToken cancellationToken);
    Task<bool> HasRole(Guid id, string role, CancellationToken cancellationToken);
}