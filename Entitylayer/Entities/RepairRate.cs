﻿using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class RepairRate
{
    public int IdRepairRate { get; set; }

    public string? RepairRateName { get; set; }

    public int? IsActive { get; set; }
}
