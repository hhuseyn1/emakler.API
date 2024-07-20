namespace EntityLayer.Entities;

public partial class Metro : BaseEntity
{
    public string? MetroName { get; set; }

    public Guid? FkIdRegion { get; set; }

    public string? Keyword01 { get; set; }

    public string? Keyword02 { get; set; }

    public string? Keyword03 { get; set; }

    public string? Keyword04 { get; set; }

    public string? Keyword05 { get; set; }
}
