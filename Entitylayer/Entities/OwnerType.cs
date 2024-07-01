using System;
using System.Collections.Generic;

namespace EntityLayer.Entities;

public partial class OwnerType
{
    public Guid IdOwnerType { get; set; }

    public string? OwnerTypeName { get; set; }

    public string? Keyword { get; set; }
}
