﻿using System;
using System.Collections.Generic;

namespace EntityLayer.Entities;

public partial class OperationType
{
    public int IdOperationType { get; set; }

    public string? OperationTypeName { get; set; }

    public string? Keyword { get; set; }
}
