using DTO.BuildingPost;
using System.Linq.Expressions;

namespace BusinessLayer.Interfaces.PostServices;

public interface IPostService
{
    Task<BuildingPostDto> GetBuildingPostByIdAsync(Guid id);
    Task<IEnumerable<BuildingPostDto>> GetAllBuildingPostsAsync();
    Task<IEnumerable<BuildingPostDto>> GetBuildingPostsByFilterAsync(Expression<Func<BuildingPostDto, bool>> filter, int pageNumber, int pageSize);
    Task<BuildingPostDto> CreateBuildingPostAsync(CreateBuildingPostDto createBuildingPostDto);
    Task<BuildingPostDto> UpdateBuildingPostAsync(Guid id, UpdateBuildingPostDto updateBuildingPostDto);
    Task<bool> DeleteBuildingPostAsync(Guid id);

}
