using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;
using YemekTarifiApp.Core.Services;
using YemekTarifiApp.Core.UnitOfWorks;

namespace YemekTarifiApp.Service.Services;

public class CommentService:GenericService<Comment>,ICommentService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserAppRepository _userAppRepository;
    public CommentService(IGenericRepository<Comment?> repository, IUnitOfWork unitOfWork, IRecipeRepository recipeRepository, ICommentRepository commentRepository, IUserAppRepository userAppRepository) : base(repository, unitOfWork)
    {
        _recipeRepository = recipeRepository;
        _commentRepository = commentRepository;
        _userAppRepository = userAppRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomResponseNoDataDto> AddComment(CommentDto commentDto, string recipeId, string userId)
    {
        var user = await _userAppRepository.Where(u => u.Id == userId && !u.IsDeleted).FirstOrDefaultAsync();
        if ( user == null)
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.UserNotFound);

        var recipe = await _recipeRepository.Where(r => r.Id == recipeId && !r.IsDeleted).FirstOrDefaultAsync();
        if (recipe == null)
            return CustomResponseNoDataDto.Fail(404, ResponseMessages.RecipeNotFound);

        var comment = new Comment()
        {
            Id = Guid.NewGuid().ToString(),
            CreatedBy = user.Id,
            CreatedAt = DateTime.Now,
            Content = commentDto.Content,
            Title = commentDto.Title,
            User = user,
            Recipe = recipe,
            RecipeId = recipe.Id
        };
        
        user.Comments.Add(comment);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }

    private async Task<CustomResponseDto<Comment>> Vote(string userId, string recipeId, string commentId, string action)
    {
        var user = await _userAppRepository.Where(u => u.Id == userId && !u.IsDeleted).FirstOrDefaultAsync();
        if ( user == null)
            return CustomResponseDto<Comment>.Fail(ResponseMessages.UserNotFound,404);

        var recipe = await _recipeRepository.Where(r => r.Id == recipeId && !r.IsDeleted).FirstOrDefaultAsync();
        if (recipe == null)
            return CustomResponseDto<Comment>.Fail(ResponseMessages.RecipeNotFound,404);

        var comment = await _commentRepository.Where(c => c.RecipeId == recipeId && c.UserId == userId && c.Id==commentId && !c.IsDeleted).FirstOrDefaultAsync();

        if (comment == null)
            return CustomResponseDto<Comment>.Fail(ResponseMessages.CommentNotFound,404);

        if (action == "up")
        {
            comment.UpVote += 1;
        }
        else
        {
            comment.DownVote += 1;
        }
        _commentRepository.Update(comment);
        await _unitOfWork.CommitAsync();
         
        return CustomResponseDto<Comment>.Success(200);
        
    }

    public async Task<CustomResponseDto<Comment>> UpVote(string userId, string recipeId,string commentId)
    {
        return await Vote(userId, recipeId, commentId,"up");
    }

    public async Task<CustomResponseDto<Comment>> DownVote(string userId, string recipeId, string commentId)
    {
        return await Vote(userId, recipeId, commentId,"down");
    }

    public async Task<CustomResponseNoDataDto> DeleteAllComments(string userId)
    {
        var comments = await _commentRepository.Where(c => c.UserId == userId && !c.IsDeleted).ToListAsync();

        foreach (var comment in comments)
        {
            await _commentRepository.RemoveAsync(comment);
        }

        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }

    public async Task<CustomResponseNoDataDto> DeleteComment(string userId, string commentId)
    {
        var comment = await _commentRepository.Where(c => c.UserId == userId && !c.IsDeleted).SingleOrDefaultAsync();
        if (comment == null)
            return CustomResponseNoDataDto.Fail(409,ResponseMessages.DuplicateEntity);

        await _commentRepository.RemoveAsync(comment);
        await _unitOfWork.CommitAsync();
        
        return CustomResponseNoDataDto.Success(200);
    }
}