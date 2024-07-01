using DTO.Building;
using EntityLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface IBuildingRepository
{
    Task AddAsync(BuildingPost buildingPost);
    Task<BuildingPost> GetByIdAsync(Guid id);
    Task<IEnumerable<BuildingPost>> GetAllAsync();
    Task<IEnumerable<BuildingPost>> GetByFilterAsync(BuildingFilter filter);
    Task UpdateAsync(BuildingPost buildingPost);
    Task DeleteAsync(Guid id);
}
