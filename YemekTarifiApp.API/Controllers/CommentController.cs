using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Services;

namespace YemekTarifiApp.API.Controllers;

public class CommentController:CustomBaseController
{
    
    private readonly ICommentService _userAppService;

    public CommentController(ICommentService userAppService)
    {
        _userAppService = userAppService;
    }

    [HttpPost("[action]/{recipeId}")]
    public async Task<IActionResult> AddComment(CommentDto commentDto,string recipeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _userAppService.AddComment(commentDto, recipeId,userId));
        
    }
}