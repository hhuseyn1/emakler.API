using EntityLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<BuildingPost>> GetAllBuildingPostsAsync();
    Task<BuildingPost> GetBuildingPostByIdAsync(Guid id);
    Task<IEnumerable<BuildingPost>> GetBuildingPostsByFilterAsync(Expression<Func<BuildingPost, bool>> predicate, int pageNumber, int pageSize);
    Task AddBuildingPostAsync(BuildingPost buildingPost);
    Task UpdateBuildingPostByIdAsync(BuildingPost buildingPost);
    Task DeleteBuildingPostByIdAsync(BuildingPost buildingPost);
}
