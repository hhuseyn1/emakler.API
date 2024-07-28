namespace BusinessLayer.Configurations;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMonths { get; set; }
}
