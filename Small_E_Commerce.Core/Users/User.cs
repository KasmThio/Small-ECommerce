namespace Small_E_Commerce.Infrastructure.Identity;

public class User
{
    public Guid Id { get; set; }
    public string KeycloakId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public List<string> Permissions { get; set; }
    
    public User(Guid id, string keycloakId, string username, string email, string firstName, string lastName, string role, List<string> permissions)
    {
        Id = id;
        KeycloakId = keycloakId;
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Permissions = permissions;
    }
    
    private User()
    {
    }
}