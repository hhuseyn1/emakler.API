namespace EntityLayer.Entities;

public partial class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public bool IsVerified { get; set; } = false;
}

