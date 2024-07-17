using DataAccessLayer.Concrete;
using DTO;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EMakler.PROAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly Context _context;

    public FavoritesController(Context context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize] 
    public async Task<IActionResult> AddToFavorite([FromBody] FavoriteDto favoriteDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var favorite = await _context.UserFavorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.BuildingId == favoriteDto.BuildingId);

        if (favorite == null)
        {
            favorite = new UserFavorite
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BuildingId = favoriteDto.BuildingId
            };

            _context.UserFavorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        return Ok(new { isFavorite = true });
    }

    [HttpDelete("{buildingId}")]
    [Authorize] 
    public async Task<IActionResult> RemoveFromFavorite(Guid buildingId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var favorite = await _context.UserFavorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.BuildingId == buildingId);

        if (favorite != null)
        {
            _context.UserFavorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        return Ok(new { isFavorite = false });
    }
}
