using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Services;

namespace YemekTarifiApp.API.Controllers;


[Authorize]
public class RecipeController : CustomBaseController
{

    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllUserRecipes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipeResponseDto= await _recipeService.GetAllUserRecipesAsync(userId);

        return CreateActionResult(recipeResponseDto);

    }

    [HttpGet("[action]/{categoryName}")]
    public async Task<IActionResult> GetAllUserRecipesByCategory( string categoryName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipeDtos = await _recipeService.GetUserRecipesByCategoryAsync(categoryName,userId);
        
        return CreateActionResult(recipeDtos);
        
    }


    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> UpdateRecipe(RecipeDto recipeDto,string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var updatedRecipe = await _recipeService.Update(recipeDto,id,userId);

        
        return CreateActionResult(updatedRecipe);

    }


    
    [HttpPost("[action]")]

    public async Task<IActionResult> Add(RecipeDto recipeDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createdRecipe = await _recipeService.CreateRecipeAsync(recipeDto, userId);
        return CreateActionResult(createdRecipe);

    }
    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> Remove (string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var removedRecipe = await _recipeService.RemoveAsync(userId, id);
        return CreateActionResult(removedRecipe);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipes = await _recipeService.GetAll(userId);
        return CreateActionResult(recipes);

    }
    
    [HttpGet("[action]/{categoryName}")]
    public async Task<IActionResult> GetAllByCategory(string categoryName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var recipes = await _recipeService.GetAllByCategory(categoryName,userId);
        return CreateActionResult(recipes);

    }
    
    [HttpDelete("[action]/{recipeId}")]
    public async Task<IActionResult> Delete(string recipeId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return CreateActionResult(await _recipeService.DeleteRecipeByIdAsync(userId, recipeId));
    }       
}