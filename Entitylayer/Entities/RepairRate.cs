using System;
using System.Collections.Generic;

namespace EntityLayer.Entities;

public partial class RepairRate
{
    public Guid IdRepairRate { get; set; }

    public string? RepairRateName { get; set; }

    public int? IsActive { get; set; }
}
