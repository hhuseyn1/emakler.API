using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces;
using DTO.Building;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingController : ControllerBase
{
    private readonly IBuildingService _buildingService;

    public BuildingController(IBuildingService buildingService)
    {
        _buildingService = buildingService;
    }

    [HttpPost("AddBuildingPost")]
    public async Task<IActionResult> AddBuildingPost([FromBody] BuildingPostDTO buildingPostDTO)
    {
        await _buildingService.AddBuildingPostAsync(buildingPostDTO);
        return Ok(new { Message = "BuildingPost added successfully." });
    }

    [HttpGet("GetBuildingPostById/{id}")]
    public async Task<IActionResult> GetBuildingPostById(Guid id)
    {
        var buildingPostDTO = await _buildingService.GetBuildingPostByIdAsync(id);
        if (buildingPostDTO == null)
            return NotFound(new { Message = "BuildingPost not found." });

        return Ok(buildingPostDTO);
    }

    [HttpGet("GetAllBuildingPosts")]
    public async Task<IActionResult> GetAllBuildingPosts()
    {
        var buildingPostsDTO = await _buildingService.GetAllBuildingPostsAsync();
        return Ok(buildingPostsDTO);
    }

    [HttpGet("GetAllBuildingPostsW/Filter")]
    public async Task<IActionResult> GetBuildingPostsByFilter([FromBody] BuildingFilter filter)
    {
        var buildingPostsDTO = await _buildingService.GetBuildingPostsByFilterAsync(filter);
        return Ok(buildingPostsDTO);
    }

    [HttpPut("UpdateBuildingPostById/{id}")]
    public async Task<IActionResult> UpdateBuildingPost(Guid id, [FromBody] BuildingPostDTO buildingPostDTO)
    {
        await _buildingService.UpdateBuildingPostAsync(id, buildingPostDTO);
        return Ok(new { Message = "BuildingPost updated successfully." });
    }

    [HttpDelete("DeleteBuildingPostById/{id}")]
    public async Task<IActionResult> DeleteBuildingPost(Guid id)
    {
        await _buildingService.DeleteBuildingPostAsync(id);
        return Ok(new { Message = "BuildingPost deleted successfully." });
    }
}
