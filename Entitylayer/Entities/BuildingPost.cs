namespace EntityLayer.Entities;

public class BuildingPost : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Building Building { get; set; }  
    public bool IsActive { get; set; } = true;
    public IList<string> ImageUrls { get; set; }
}
