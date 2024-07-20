namespace BusinessLayer.Configurations;

public class JwtSettings
{
    public string Secret { get; set; }
    public int ExpiryDay { get; set; }
}
