namespace Notes_with_tagging.Identity
{
    public record class JwtOptions(
    string Issuer,
    string Audience,
    string SigningKey,
    int ExpirationSeconds
    );
}
