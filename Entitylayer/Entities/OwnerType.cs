﻿using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class OwnerType
{
    public int IdOwnerType { get; set; }

    public string? OwnerTypeName { get; set; }

    public string? Keyword { get; set; }
}
