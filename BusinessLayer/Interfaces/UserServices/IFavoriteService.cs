using DTO.Building;
using DTO.User;

namespace BusinessLayer.Interfaces.UserServices;

public interface IFavoriteService
{
    Task AddToFavoritesAsync(AddToFavoritesRequest request);
    Task RemoveFromFavoritesAsync(Guid id);
    Task<IEnumerable<BuildingPostDTO>> GetUserFavoritesAsync(Guid userId);
}
