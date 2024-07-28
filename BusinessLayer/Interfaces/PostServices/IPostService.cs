using DTO.BuildingPost;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace BusinessLayer.Interfaces.PostServices;

public interface IPostService
{
    Task<BuildingPost> GetBuildingPostByIdAsync(Guid id);
    Task<IEnumerable<BuildingPost>> GetAllBuildingPostsAsync();
    Task<IEnumerable<BuildingPost>> GetBuildingPostsByFilterAsync(Expression<Func<BuildingPostDto, bool>> filter, int pageNumber, int pageSize);
    Task<BuildingPost> CreateBuildingPostAsync(BuildingPostDto buildingPostDto, IList<IFormFile> files);
    Task<BuildingPost> UpdateBuildingPostByIdAsync(Guid id, BuildingPostDto buildingPostDto);
    Task<bool> DeleteBuildingPostByIdAsync(Guid id);
}
