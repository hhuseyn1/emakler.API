using BusinessLayer.Interfaces.UserServices;
using DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace EMakler.PROAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavController : ControllerBase
{
    private readonly IFavoriteService _favService;
    private readonly ILogger<FavController> _logger;

    public FavController(IFavoriteService favService, ILogger<FavController> logger)
    {
        _favService = favService;
        _logger = logger;
    }

    [HttpPost("Favorites")]
    public async Task<IActionResult> AddToFavorites(AddToFavoritesRequest request)
    {
        try
        {
            await _favService.AddToFavoritesAsync(request);
            return Ok("Added to favorites.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding to favorites.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromFavorites(Guid id)
    {
        try
        {
            await _favService.RemoveFromFavoritesAsync(id);
            return Ok("Removed from favorites.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing from favorites.");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserFavorites(Guid userId)
    {
        try
        {
            var favorites = await _favService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user favorites.");
            return StatusCode(500, "Internal server error.");
        }
    }
}
