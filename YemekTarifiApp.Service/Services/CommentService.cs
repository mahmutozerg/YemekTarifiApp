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
}