namespace DTO.BuildingPost;

public class BuildingPostDto
{
    public Guid Id { get; set; }
    public BuildingDto Building { get; set; }
    public bool IsActive { get; set; } = true;
}
