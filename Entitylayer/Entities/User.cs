using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserMail { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string? OtpCode { get; set; }
}
