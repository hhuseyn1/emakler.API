namespace EntityLayer.Entities;

public class BuildingPost : BaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Building Building { get; set; }  
    public bool IsActive { get; set; } = true;
    public List<string> ImagePaths { get; set; } = new List<string>();
}
