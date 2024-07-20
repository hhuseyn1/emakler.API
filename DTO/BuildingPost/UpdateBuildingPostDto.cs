namespace DTO.BuildingPost;

public class UpdateBuildingPostDto
{
    public BuildingDto Building { get; set; }
    public bool IsActive { get; set; } = true;
}
