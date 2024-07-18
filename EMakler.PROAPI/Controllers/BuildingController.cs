using BusinessLayer.Interfaces;
using DTO.Building;
using Microsoft.AspNetCore.Mvc;

namespace EMakler.PROAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BuildingController : ControllerBase
{
    private readonly IBuildingService _buildingService;
    private readonly ILogger<BuildingController> _logger;

    public BuildingController(IBuildingService buildingService, ILogger<BuildingController> logger)
    {
        _buildingService = buildingService;
        _logger = logger;
    }

    [HttpPost("AddBuildingPost")]
    public async Task<IActionResult> AddBuildingPost([FromBody] BuildingPostDTO buildingPostDTO)
    {
        try
        {
            await _buildingService.AddBuildingPostAsync(buildingPostDTO);
            _logger.LogInformation("BuildingPost added successfully.");
            return Ok(new { Message = "BuildingPost added successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding BuildingPost.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("GetBuildingPostById/{id}")]
    public async Task<IActionResult> GetBuildingPostById(Guid id)
    {
        try
        {
            var buildingPostDTO = await _buildingService.GetBuildingPostByIdAsync(id);
            if (buildingPostDTO == null)
            {
                _logger.LogWarning($"BuildingPost not found for ID: {id}");
                return NotFound(new { Message = "BuildingPost not found." });
            }

            return Ok(buildingPostDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving BuildingPost.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("GetAllBuildingPosts")]
    public async Task<IActionResult> GetAllBuildingPosts()
    {
        try
        {
            var buildingPostsDTO = await _buildingService.GetAllBuildingPostsAsync();
            return Ok(buildingPostsDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving BuildingPosts.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("GetAllBuildingPostsW/Filter")]
    public async Task<IActionResult> GetBuildingPostsByFilter([FromBody] BuildingFilter filter)
    {
        try
        {
            var buildingPostsDTO = await _buildingService.GetBuildingPostsByFilterAsync(filter);
            return Ok(buildingPostsDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving BuildingPosts by filter.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("UpdateBuildingPostById/{id}")]
    public async Task<IActionResult> UpdateBuildingPost(Guid id, [FromBody] BuildingPostDTO buildingPostDTO)
    {
        try
        {
            await _buildingService.UpdateBuildingPostAsync(id, buildingPostDTO);
            _logger.LogInformation("BuildingPost updated successfully.");
            return Ok(new { Message = "BuildingPost updated successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating BuildingPost.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("DeleteBuildingPostById/{id}")]
    public async Task<IActionResult> DeleteBuildingPost(Guid id)
    {
        try
        {
            await _buildingService.DeleteBuildingPostAsync(id);
            _logger.LogInformation("BuildingPost deleted successfully.");
            return Ok(new { Message = "BuildingPost deleted successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting BuildingPost.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("Paged")]
    public async Task<IActionResult> GetPagedBuildings(int pageNumber, int pageSize)
    {
        try
        {
            var pagedBuildings = await _buildingService.GetBuildingPostsByPaginationAsync(pageNumber, pageSize);
            return Ok(pagedBuildings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged buildings.");
            return StatusCode(500, "Internal server error.");
        }
    }
    [HttpGet("Search")]
    public async Task<IActionResult> SearchBuildingPosts([FromQuery] string searchTerm)
    {
        try
        {
            var buildingPosts = await _buildingService.SearchBuildingPostsAsync(searchTerm);
            return Ok(buildingPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for BuildingPosts.");
            return StatusCode(500, "Internal server error.");
        }
    }
}

