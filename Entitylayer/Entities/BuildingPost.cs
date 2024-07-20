namespace EntityLayer.Entities;

public class BuildingPost : BaseEntity
{
    public Building Building { get; set; }  
    public bool IsActive { get; set; } = true;
}
