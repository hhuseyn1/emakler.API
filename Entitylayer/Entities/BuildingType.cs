﻿using System;
using System.Collections.Generic;

namespace EntityLayer.Entities;

public partial class BuildingType
{
    public int IdBuildingType { get; set; }

    public string? BuildingTypeName { get; set; }

    public string? Keyword { get; set; }
}
