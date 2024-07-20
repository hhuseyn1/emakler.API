using EntityLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces;

public interface IPostRepository
{
    Task<BuildingPost> GetBuildingPostByIdAsync(Guid id);
    Task<IEnumerable<BuildingPost>> GetAllBuildingPostsAsync();
    Task<IEnumerable<BuildingPost>> GetBuildingPostsByFilterAsync(Expression<Func<BuildingPost, bool>> predicate, int pageNumber, int pageSize);
    Task AddBuildingPostAsync(BuildingPost buildingPost);
    Task UpdateBuildingPostAsync(BuildingPost buildingPost);
    Task DeleteBuildingPostAsync(BuildingPost buildingPost);
}
