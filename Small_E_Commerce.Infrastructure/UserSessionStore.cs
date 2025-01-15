using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Identity.Users;
using Small_E_Commerce.Products;
using IdentityDbContext = Small_E_Commerce.Identity.IdentityDbContext;

namespace Small_E_Commerce.Infrastructure;

public class UserSessionStore(IdentityDbContext identityDbContext) : IUserSessionStore
{
    public async Task CreateAsync(UserSession userSession, CancellationToken cancellationToken = default)
    {
        await identityDbContext
            .Set<UserSession>()
            .AddAsync(userSession, cancellationToken);

        await identityDbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<UserSession?> GetAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await identityDbContext
            .Set<UserSession>()
            .Where(s => s.SessionId == sessionId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(string sessionId, Guid userId)> RefreshAsync(string sessionId, DateTimeOffset updatedAt, CancellationToken cancellationToken = default)
    {
        var userSession = await GetAsync(sessionId, cancellationToken);

        if (userSession is null)
        {
            throw new DomainException("User session not found");
        }

        var newSessionId = Guid.NewGuid().ToString();

        userSession.Refresh(newSessionId, updatedAt);

        await identityDbContext.SaveChangesAsync(cancellationToken);

        return (newSessionId, userSession.UserId);
    }
}