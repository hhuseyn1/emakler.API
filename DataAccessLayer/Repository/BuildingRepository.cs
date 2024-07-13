using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using DTO.Building;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccessLayer.Repository;

public class BuildingRepository : IBuildingRepository
{
    private readonly Context _context;

    public BuildingRepository(Context context)
    {
        _context = context;
    }

    public async Task AddAsync(BuildingPost buildingPost)
    {
        await _context.BuildingPosts.AddAsync(buildingPost);
        await _context.SaveChangesAsync();
    }

    public async Task<BuildingPost> GetByIdAsync(Guid id)
    {
        return await _context.BuildingPosts.Include(bp => bp.Building).FirstOrDefaultAsync(bp => bp.Building.Id == id);
    }

    public async Task<IEnumerable<BuildingPost>> GetAllAsync()
    {
        return await _context.BuildingPosts.Include(bp => bp.Building).ToListAsync();
    }

    public async Task<IEnumerable<BuildingPost>> GetByFilterAsync(BuildingFilter filter)
    {
        var query = _context.BuildingPosts.AsQueryable();

        if (filter.MinPrice.HasValue)
            query = query.Where(bp => bp.Building.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(bp => bp.Building.Price <= filter.MaxPrice.Value);

        if (!string.IsNullOrEmpty(filter.Address))
            query = query.Where(bp => bp.Building.City.Contains(filter.Address) || bp.Building.District.Contains(filter.Address));

        return await query.Include(bp => bp.Building).ToListAsync();
    }

    public async Task UpdateAsync(BuildingPost buildingPost)
    {
        _context.BuildingPosts.Update(buildingPost);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var buildingPost = await GetByIdAsync(id);
        if (buildingPost != null)
        {
            _context.BuildingPosts.Remove(buildingPost);
            await _context.SaveChangesAsync();
        }
    }

    
    public async Task<IEnumerable<BuildingPost>> GetByPaginationAsync(int pageNumber, int pageSize)
    {
        var query = _context.BuildingPosts.AsQueryable();

        var totalItemCount = await query.CountAsync();

        return await query
            .Include(bp => bp.Building)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    }

    public async Task<IEnumerable<BuildingPost>> SearchAsync(string searchTerm)
    {
        return await _context.BuildingPosts.Include(bp => bp.Building)
                             .Where(bp => bp.Building.Village.Contains(searchTerm) || bp.Building.District.Contains(searchTerm))
                             .ToListAsync();
                             
    }
}
