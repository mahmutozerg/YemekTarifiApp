using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Services;

public interface ICommentService:IGenericService<Comment>
{
    Task<CustomResponseNoDataDto> AddComment(CommentDto commentDto, string recipeId,string userId);
    Task<CustomResponseDto<Comment>> UpVote(string userId, string recipeId, string commentId);
    Task<CustomResponseDto<Comment>> DownVote(string userId, string recipeId, string commentId);

    Task<CustomResponseNoDataDto> DeleteAllComments(string userId);
    Task<CustomResponseNoDataDto> DeleteComment(string userId,string commentId);
    

}