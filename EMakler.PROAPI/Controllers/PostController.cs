using BusinessLayer.Interfaces.PostServices;
using DTO.BuildingPost;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _buildingPostService;

    public PostController(IPostService buildingPostService)
    {
        _buildingPostService = buildingPostService;
    }

    [HttpGet("GetAllBuildingPosts")]
    public async Task<IActionResult> GetAllBuildingPosts()
    {
        var buildingPosts = await _buildingPostService.GetAllBuildingPostsAsync();
        return Ok(buildingPosts);
    }

    [HttpGet("GetBuildingPostsByFilter")]
    public async Task<IActionResult> GetBuildingPostsByFilter([FromQuery] string metro, [FromQuery] string city, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        Expression<Func<BuildingPostDto, bool>> filter = bp => (string.IsNullOrEmpty(metro) || bp.Building.Metro.Contains(metro)) &&
                                                               (string.IsNullOrEmpty(city) || bp.Building.City.Contains(city));

        var buildingPosts = await _buildingPostService.GetBuildingPostsByFilterAsync(filter, pageNumber, pageSize);
        return Ok(buildingPosts);
    }

    [HttpGet("GetBuildingPostById/{id}")]
    public async Task<IActionResult> GetBuildingPostById(Guid id)
    {
        var buildingPost = await _buildingPostService.GetBuildingPostByIdAsync(id);
        return Ok(buildingPost);
    }

    [HttpPost("CreateBuildingPost")]
    public async Task<IActionResult> CreateBuildingPost([FromForm] CreateBuildingPostDto createBuildingPostDto, [FromForm] IList<IFormFile> files)
    {
        var buildingPost = await _buildingPostService.CreateBuildingPostAsync(createBuildingPostDto, files);
        return CreatedAtAction(nameof(GetBuildingPostById), new { id = buildingPost.Id }, buildingPost);
    }

    [HttpPut("UpdateBuildingPostbyId/{id}")]
    public async Task<IActionResult> UpdateBuildingPostbyId(Guid id, [FromBody] UpdateBuildingPostDto updateBuildingPostDto)
    {
        var buildingPost = await _buildingPostService.UpdateBuildingPostAsync(id, updateBuildingPostDto);
        return Ok(buildingPost);
    }

    [HttpDelete("DeleteBuildingPostbyId/{id}")]
    public async Task<IActionResult> DeleteBuildingPostbyId(Guid id)
    {
        await _buildingPostService.DeleteBuildingPostAsync(id);
        return NoContent();
    }
}
