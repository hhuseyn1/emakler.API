using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Entities;

public class BuildingPost
{
    public Guid BuildingId { get; set; }
    public Building Building { get; set; }  
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
