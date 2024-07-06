
namespace DTO.Building;
public class BuildingPostDTO
{
    public Guid BuildingId { get; set; }  
    public BuildingDTO Building { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}