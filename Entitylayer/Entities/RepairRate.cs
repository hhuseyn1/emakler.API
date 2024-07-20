namespace EntityLayer.Entities;

public partial class RepairRate : BaseEntity
{
    public string? RepairRateName { get; set; }

    public int? IsActive { get; set; }
}
