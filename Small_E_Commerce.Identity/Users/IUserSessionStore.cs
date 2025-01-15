namespace Small_E_Commerce.Identity.Users;

public interface IUserSessionStore
{
    Task CreateAsync(UserSession userSession, CancellationToken cancellationToken = default);
    Task<UserSession?> GetAsync(string sessionId, CancellationToken cancellationToken = default);
    Task<(string sessionId, Guid userId)> RefreshAsync(string sessionId, DateTimeOffset updatedAt, CancellationToken cancellationToken = default);
}