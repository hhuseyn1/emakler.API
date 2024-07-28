namespace DTO.BuildingPost;

public class BuildingPostDto
{
    public BuildingDto Building { get; set; }
    public bool IsActive { get; set; } = true;
    public IEnumerable<string> ImageUrls { get; set; }
}
