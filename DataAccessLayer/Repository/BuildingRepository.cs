using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using DTO.Building;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository;

public class BuildingRepository : IBuildingRepository
{
    private readonly Context _context;
    private readonly ILogger<BuildingRepository> _logger;

    public BuildingRepository(Context context, ILogger<BuildingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(BuildingPost buildingPost)
    {
        try
        {
            await _context.BuildingPosts.AddAsync(buildingPost);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Building post added successfully with ID: {buildingPost.BuildingId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding the building post");
            throw;
        }
    }

    public async Task<BuildingPost> GetByIdAsync(Guid id)
    {
        try
        {
            var buildingPost = await _context.BuildingPosts
                .Include(bp => bp.Building)
                .FirstOrDefaultAsync(bp => bp.Building.Id == id);

            if (buildingPost == null)
            {
                _logger.LogWarning($"Building post not found with ID: {id}");
            }

            return buildingPost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving building post by ID: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPost>> GetAllAsync()
    {
        try
        {
            return await _context.BuildingPosts
                .Include(bp => bp.Building)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all building posts");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPost>> GetByFilterAsync(BuildingFilter filter)
    {
        try
        {
            var query = _context.BuildingPosts.AsQueryable();

            if (filter.MinPrice.HasValue)
                query = query.Where(bp => bp.Building.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(bp => bp.Building.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrEmpty(filter.Address))
                query = query.Where(bp => bp.Building.City.Contains(filter.Address) || bp.Building.District.Contains(filter.Address));

            return await query
                .Include(bp => bp.Building)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying filter to building posts");
            throw;
        }
    }

    public async Task UpdateAsync(BuildingPost buildingPost)
    {
        try
        {
            _context.BuildingPosts.Update(buildingPost);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Building post updated successfully with ID: {buildingPost.BuildingId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating building post with ID: {buildingPost.BuildingId}");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var buildingPost = await GetByIdAsync(id);
            if (buildingPost != null)
            {
                _context.BuildingPosts.Remove(buildingPost);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Building post deleted successfully with ID: {id}");
            }
            else
            {
                _logger.LogWarning($"Building post not found with ID: {id}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting building post with ID: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPost>> GetByPaginationAsync(int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.BuildingPosts.AsQueryable();

            var totalItemCount = await query.CountAsync();

            return await query
                .Include(bp => bp.Building)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while paginating building posts");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPost>> SearchAsync(string searchTerm)
    {
        try
        {
            return await _context.BuildingPosts
                .Include(bp => bp.Building)
                .Where(bp => bp.Building.Village.Contains(searchTerm) || bp.Building.District.Contains(searchTerm))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while searching building posts with term: {searchTerm}");
            throw;
        }
    }
}
