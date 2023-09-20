using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Services;

public interface ICommentService:IGenericService<Comment>
{
    Task<CustomResponseNoDataDto> AddComment(CommentDto commentDto, string recipeId,string userId);
}