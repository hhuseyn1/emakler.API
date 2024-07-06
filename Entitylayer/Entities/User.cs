using System;
using System.Collections.Generic;

namespace EntityLayer.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string UserMail { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string? OtpCode { get; set; }

    public DateTime OtpCreatedTime { get; set; }
    public bool IsValidate { get; set; }
    public string UserPassword { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
}

