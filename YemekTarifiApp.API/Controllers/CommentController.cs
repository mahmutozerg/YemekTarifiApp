using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Services;

namespace YemekTarifiApp.API.Controllers;

[Authorize]
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
    [HttpPost("[action]")]
    public async Task<IActionResult> Upvote(VoteDto commentDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _userAppService.UpVote(userId, commentDto.RecipeId,
            commentDto.CommentId));

    }
    [HttpPost("[action]")]
    public async Task<IActionResult> DownVote(VoteDto commentDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _userAppService.DownVote(userId, commentDto.RecipeId,
            commentDto.CommentId));

    }
    
    [HttpDelete("[action]/{commentId}")]
    public async Task<IActionResult> DeleteComment(string commentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _userAppService.DeleteComment(userId, commentId));

    }
}