using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Identity.Users;

public class UserSession : EntityBase<long>
{
    public Guid UserId { get; private set; }
    public string SessionId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt = DateTimeOffset.UtcNow;

    public UserSession(Guid userId, string sessionId, DateTimeOffset createdAt)
    {
        UserId = userId;
        SessionId = sessionId;
        CreatedAt = createdAt;
    }

    private UserSession()
    {
    }

    public void Refresh(string sessionId, DateTimeOffset updatedAt)
    {
        SessionId = sessionId;
        UpdatedAt = updatedAt;
    }

}