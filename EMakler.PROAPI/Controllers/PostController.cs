using BusinessLayer.Interfaces.PostServices;
using DTO.BuildingPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("GetAllBuildingPosts")]
    public async Task<IActionResult> GetAllBuildingPosts()
    {
        var posts = await _postService.GetAllBuildingPostsAsync();
        return Ok(posts);
    }

    [HttpGet("GetBuildingPostById/{id}")]
    public async Task<IActionResult> GetBuildingPostById(Guid id)
    {
        var post = await _postService.GetBuildingPostByIdAsync(id);
        return Ok(post);
    }

    [HttpGet("GetBuildingPostsByFilter")]
    public async Task<IActionResult> GetBuildingPostsByFilter([FromQuery] string metro, [FromQuery] string city, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        Expression<Func<BuildingPostDto, bool>> filter = bp => (string.IsNullOrEmpty(metro) || bp.Building.Metro.Contains(metro)) &&
                                                               (string.IsNullOrEmpty(city) || bp.Building.City.Contains(city));

        var posts = await _postService.GetBuildingPostsByFilterAsync(filter, pageNumber, pageSize);
        return Ok(posts);
    }

    [Authorize]
    [HttpPost("CreateBuildingPost")]
    public async Task<IActionResult> CreateBuildingPost([FromForm] BuildingPostDto createBuildingPostDto, [FromForm] IList<IFormFile> files)
    {
        var post = await _postService.CreateBuildingPostAsync(createBuildingPostDto, files);
        return CreatedAtAction(nameof(GetBuildingPostById), new { id = post.Id }, post);
    }

    [Authorize]
    [HttpPut("UpdateBuildingPostById/{id}")]
    public async Task<IActionResult> UpdateBuildingPost(Guid id, [FromBody] BuildingPostDto updateBuildingPostDto)
    {
        var post = await _postService.UpdateBuildingPostByIdAsync(id, updateBuildingPostDto);
        return Ok(post);
    }

    [Authorize]
    [HttpDelete("DeleteBuildingPostById/{id}")]
    public async Task<IActionResult> DeleteBuildingPost(Guid id)
    {
        await _postService.DeleteBuildingPostByIdAsync(id);
        return NoContent();
    }
}
