using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Mappers;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;
using YemekTarifiApp.Core.Services;
using YemekTarifiApp.Core.UnitOfWorks;
using YemekTarifiApp.Service.Services;


public class RecipeService:GenericService<Recipe>,IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAppRepository _userRepository;
    private readonly ICommentRepository _commentRepository;

    public async Task<CustomResponseListDataDto<RecipeResponseDto>> GetAllUserRecipesAsync(string userId)
    {
        
        var recipes = await _recipeRepository.Where(r=> r!.UserId == userId && !r.IsDeleted).AsNoTracking().ToListAsync();
        
        return CustomResponseListDataDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDtoList(recipes),200);
    }
    public async Task<CustomResponseListDataDto<RecipeResponseDto>> GetUserRecipesByCategoryAsync(string listName, string userId)
    {
        
        var recipes =  await _recipeRepository.GetByCategoryNameAsync(listName,userId);
        return CustomResponseListDataDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDtoList(recipes),200);
    }

    public async Task<CustomResponseListDataDto<RecipeResponseDto>> GetAll(string userId)
    {
        var recipes = await _recipeRepository.Where(r=> !r.IsDeleted).AsNoTracking().ToListAsync();
        return CustomResponseListDataDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDtoList(recipes),200);

    }

    public async Task<CustomResponseListDataDto<RecipeResponseDto>> GetAllByCategory(string listName, string userId)
    {
        var recipes = await _recipeRepository.Where(r=> !r.IsDeleted && r.CategoryName == listName).AsNoTracking().ToListAsync();
        return CustomResponseListDataDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDtoList(recipes),200);    }
    

    public async Task<CustomResponseDto<RecipeResponseDto>> Update(RecipeDto recipeDto, string id, string userId)
    {


        var recipes = await _recipeRepository.Where(r => r!.UserId == userId && r.Id == id &&!r.IsDeleted).FirstOrDefaultAsync();

        if (recipes != null)
        {
            recipes.RecipeContext = recipeDto.RecipeContext == string.Empty ? recipes.RecipeContext : recipeDto.RecipeContext;
            recipes.Title = recipeDto.Title == string.Empty ? recipes.Title : recipeDto.Title;
            recipes.CategoryName = recipeDto.CategoryName == string.Empty ? recipes.CategoryName : recipeDto.CategoryName;
            await Update(recipes);
            return  CustomResponseDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDto(recipes),200);

        }

        return CustomResponseDto<RecipeResponseDto>.Fail(ResponseMessages.RecipeNotFound, 404);
    }

    public async Task<CustomResponseDto<RecipeResponseDto>> CreateRecipeAsync(RecipeDto recipeDto, string userId)
    {
        var user = await _userRepository.Where(u => u.Id == userId).FirstOrDefaultAsync();

        if (user is null)
            return  CustomResponseDto<RecipeResponseDto>.Fail(ResponseMessages.UserNotFound,404);


        var recipe  = new Recipe()
        {
            Id = Guid.NewGuid().ToString(),
            User = user,
            UserId= userId,
            CategoryName = recipeDto.CategoryName,
            CreatedAt = DateTime.Now,
            CreatedBy = userId,
            Title = recipeDto.Title,
            RecipeContext = recipeDto.RecipeContext,
            IsDeleted = false
        };

        user.Recipes.Add(recipe);
        //await _recipeRepository.AddAsync(recipe);
        await  _unitOfWork.CommitAsync();
        return  CustomResponseDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDto(recipe),200);
    }
    
    public   async Task<CustomResponseNoDataDto> RemoveAsync(string userId, string id)
    {
        var recipe = await _recipeRepository.Where(r => r!.UserId == userId && r.Id == id && !r.IsDeleted).FirstOrDefaultAsync();
        
        if (recipe == null)
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.RecipeNotFound);


        await _recipeRepository.RemoveAsync(recipe);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(204);
    }

    public async Task<CustomResponseNoDataDto> DeleteUserAllRecipesAsync(string userId)
    {
        var recipes =  _recipeRepository.Where(r => r.UserId == userId && !r.IsDeleted);
        if (recipes == null)
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.RecipeNotFound);
        
        foreach (var recipe in recipes)
        {
            await _recipeRepository.RemoveAsync(recipe);
        }

        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }

    public async Task<CustomResponseNoDataDto> DeleteRecipeByIdAsync(string userId, string recipeId)
    {
        var recipe =  await _recipeRepository.Where(r => r.UserId == userId && !r.IsDeleted).Include(r=>r.Comments).FirstOrDefaultAsync();
        if (recipe is null)
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.RecipeNotFound);

        await _recipeRepository.RemoveAsync(recipe);
        foreach (var comment in recipe.Comments)
        {
            await _commentRepository.RemoveAsync(comment);

        }
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }


    public RecipeService(IGenericRepository<Recipe?> repository, IUnitOfWork unitOfWork, IRecipeRepository recipeRepository, IUserAppRepository userRepository, ICommentRepository commentRepository) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository;
    }
}