namespace Small_E_Commerce.Identity;

public sealed class AuthSettings
{
    public const string SectionName = "AuthSettings";

    public string Key { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
}