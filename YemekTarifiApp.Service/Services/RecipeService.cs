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

    public async Task<CustomResponseListDataDto<RecipeResponseDto>> GetAllAsync(string userId)
    {
        
        var recipes = await _recipeRepository.Where(t=> t!.UserId == userId && !t.IsDeleted).AsNoTracking().ToListAsync();
        
        return CustomResponseListDataDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDtoList(recipes),200);
    }
    public async Task<CustomResponseListDataDto<RecipeResponseDto>> GetByCategoryAsync(string listName, string userId)
    {
        
        var recipes =  await _recipeRepository.GetByCategoryNameAsync(listName,userId);
        return CustomResponseListDataDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDtoList(recipes),200);
    }
    
    public async Task<CustomResponseDto<RecipeResponseDto>> Update(RecipeDto recipeDto, string id, string userId)
    {


        var recipes = await _recipeRepository.Where(t => t!.UserId == userId && t.Id == id &&!t.IsDeleted).FirstOrDefaultAsync();

        if (recipes != null)
        {
            recipes.RecipeContext = recipeDto.RecipeContext == string.Empty ? recipes.RecipeContext : recipeDto.RecipeContext;
            recipes.Title = recipeDto.Title == string.Empty ? recipes.Title : recipeDto.Title;
            recipes.CategoryName = recipeDto.CategoryName == string.Empty ? recipes.CategoryName : recipeDto.CategoryName;
            await Update(recipes);
            return  CustomResponseDto<RecipeResponseDto>.Success(RecipeMapper.ToResponseDto(recipes),200);

        }
        throw new Exception(ResponseMessages.SessionInvalidAccesRecipe);
    }

    public async Task<CustomResponseDto<RecipeResponseDto>> CreateRecipe(RecipeDto recipeDto, string userId)
    {
        var user = await _userRepository.Where(x => x.Id == userId).FirstOrDefaultAsync();

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
    
    public   async Task<CustomResponseNoDataDto> Remove(string userId, string id)
    {

        
        var recipe = await _recipeRepository.Where(t => t!.UserId == userId && t.Id == id && !t.IsDeleted).FirstOrDefaultAsync();
        
        if (recipe != null)
        {
            recipe.IsDeleted = true;
            await Update(recipe);
            return CustomResponseNoDataDto.Success(204);
        }

        throw new Exception(ResponseMessages.SessionInvalidAccesRecipe);
    }

    public async Task<CustomResponseNoDataDto> DeleteUserRecipes(string userId)
    {
        var recipes =  _recipeRepository.Where(x => x.UserId == userId && !x.IsDeleted);
        if (recipes is null)
        {
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.RecipeNotFound);
        }
        foreach (var recipe in recipes)
        {
            recipe.IsDeleted = true;
        }

        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }


    public RecipeService(IGenericRepository<Recipe?> repository, IUnitOfWork unitOfWork, IRecipeRepository recipeRepository, IUserAppRepository userRepository) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
    }
}