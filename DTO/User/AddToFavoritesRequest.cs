namespace DTO.User;

public class AddToFavoritesRequest
{
    public Guid UserId { get; set; }
    public Guid BuildingPostId { get; set; }
}
