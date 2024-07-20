namespace DTO.BuildingPost;

public class CreateBuildingPostDto
{
    public BuildingDto Building { get; set; }
    public bool IsActive { get; set; } = true;
}
