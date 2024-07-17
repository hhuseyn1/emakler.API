namespace EntityLayer.Entities;

public class UserFavorite
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Guid BuildingId { get; set; }
}
