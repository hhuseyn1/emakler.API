namespace EntityLayer.Entities;

public partial class Property : BaseEntity
{
    public Guid? FkIdSource { get; set; }
    public Guid? FkIdLink { get; set; }
    public string? Code { get; set; }
    public Guid? FkIdPropertyType { get; set; }
    public Guid? FkIdOperationType { get; set; }
    public Guid? FkIdCity { get; set; }
    public string? Address { get; set; }
    public Guid? FkIdDocument { get; set; }
    public double? Price { get; set; }
    public Guid? FkIdCurrency { get; set; }
    public string? Data { get; set; }
    public double? Area { get; set; }
    public double? GeneralArea { get; set; }
    public int? Floor { get; set; }
    public int? FloorOf { get; set; }
    public Guid? FkIdRoom { get; set; }
    public Guid? FkIdBuildingType { get; set; }
    public double? UnitPrice { get; set; }
    public string? EX { get; set; }
    public string? EY { get; set; }
    public string? CpName { get; set; }
    public string? CpPhoneNumber01 { get; set; }
    public string? CpPhoneNumber02 { get; set; }
    public string? CpPhoneNumber03 { get; set; }
    public Guid? FkIdOwnerType { get; set; }
    public string? Images { get; set; }
    public DateTime? InsertDate { get; set; }
    public int? UploadStatus { get; set; }
    public string? UploadMessage { get; set; }
    public Guid? FkIdMetro { get; set; }
    public int? ApprovmentStatus { get; set; }
    public string? ApprovmentMessage { get; set; }
    public Guid? FkIdRepair { get; set; }
    public Guid? FkIdTarget { get; set; }
}
