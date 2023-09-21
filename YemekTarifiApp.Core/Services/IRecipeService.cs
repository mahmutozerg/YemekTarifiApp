using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Services;

public interface IRecipeService:IGenericService<Recipe>
{
    Task<CustomResponseListDataDto<RecipeResponseDto>> GetAllUserRecipesAsync(string userId);
    Task<CustomResponseDto<RecipeResponseDto>> CreateRecipeAsync(RecipeDto recipeDto, string userId);
    Task<CustomResponseListDataDto<RecipeResponseDto>> GetUserRecipesByCategoryAsync(string listName, string userId);
    Task<CustomResponseListDataDto<RecipeResponseDto>> GetAll(string userId);
    Task<CustomResponseListDataDto<RecipeResponseDto>> GetAllByCategory(string listName, string userId);

    Task<CustomResponseDto<RecipeResponseDto>> Update(RecipeDto recipeDto, string id, string userId);
    Task<CustomResponseNoDataDto> RemoveAsync(string userId, string id);
    Task<CustomResponseNoDataDto>  DeleteUserAllRecipesAsync(string userId);
    Task<CustomResponseNoDataDto>  DeleteRecipeByIdAsync(string userId,string recipeId);

}