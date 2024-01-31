namespace Notes_with_tagging.Models
{
    public record class JwtOptions(
    string Issuer,
    string Audience,
    string SigningKey,
    int ExpirationSeconds
    );
}
