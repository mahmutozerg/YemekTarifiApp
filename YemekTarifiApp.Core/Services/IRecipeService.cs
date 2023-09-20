using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Services;

public interface IRecipeService:IGenericService<Recipe>
{
    Task<CustomResponseListDataDto<RecipeResponseDto>> GetAllAsync(string userId);
    Task<CustomResponseDto<RecipeResponseDto>> CreateRecipe(RecipeDto recipeDto, string userId);
    Task<CustomResponseListDataDto<RecipeResponseDto>> GetByCategoryAsync(string listName, string userId);
    Task<CustomResponseDto<RecipeResponseDto>> Update(RecipeDto recipeDto, string id, string userId);
    Task<CustomResponseNoDataDto> Remove(string userId, string id);
    Task<CustomResponseNoDataDto>  DeleteUserRecipes(string userId);

}