using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repository;

public class PostRepository : IPostRepository
{
    private readonly Context _context;

    public PostRepository(Context context)
    {
        _context = context;
    }

    public async Task<BuildingPost> GetBuildingPostByIdAsync(Guid id)
    {
        return await _context.BuildingPosts
                             .Include(bp => bp.Building)
                             .FirstOrDefaultAsync(bp => bp.Id == id);
    }

    public async Task<IEnumerable<BuildingPost>> GetAllBuildingPostsAsync()
    {
        return await _context.BuildingPosts
                             .Include(bp => bp.Building)
                             .ToListAsync();
    }

    public async Task<IEnumerable<BuildingPost>> GetBuildingPostsByFilterAsync(Expression<Func<BuildingPost, bool>> predicate, int pageNumber, int pageSize)
    {
        return await _context.BuildingPosts
                             .Include(bp => bp.Building)
                             .Where(predicate)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();
    }

    public async Task AddBuildingPostAsync(BuildingPost buildingPost)
    {
        await _context.BuildingPosts.AddAsync(buildingPost);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBuildingPostAsync(BuildingPost buildingPost)
    {
        _context.BuildingPosts.Update(buildingPost);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBuildingPostAsync(BuildingPost buildingPost)
    {
        _context.BuildingPosts.Remove(buildingPost);
        await _context.SaveChangesAsync();
    }
}
