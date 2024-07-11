using DTO.Building;

namespace BusinessLayer.Interfaces;

public interface IBuildingService
{
    Task AddBuildingPostAsync(BuildingPostDTO buildingPostDTO);
    Task<BuildingPostDTO> GetBuildingPostByIdAsync(Guid id);
    Task<IEnumerable<BuildingPostDTO>> GetAllBuildingPostsAsync();
    Task<IEnumerable<BuildingPostDTO>> GetBuildingPostsByFilterAsync(BuildingFilter filter);
    Task UpdateBuildingPostAsync(Guid id, BuildingPostDTO buildingPostDTO);
    Task DeleteBuildingPostAsync(Guid id);
    Task<IEnumerable<BuildingPostDTO>> GetBuildingPostsByPaginationAsync(int  pageNumber, int pageSize);
}
