using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemekTarifiApp.Core.Services;

namespace YemekTarifiApp.API.Controllers;

[Authorize]
public class FavoriteController : CustomBaseController
{
    private readonly IFavoriteService _favoriteService;


    public FavoriteController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> ToggleFavorites(string recipeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _favoriteService.ToggleFavorites(recipeId, userId));

    }

    [HttpGet("[action]/{pageId}")]
    public async Task<IActionResult> GetRecentFavorites(int pageId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _favoriteService.GetFavorites(userId,pageId));


    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserFavorites()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _favoriteService.DeleteUserFavorites(userId));
    }
}