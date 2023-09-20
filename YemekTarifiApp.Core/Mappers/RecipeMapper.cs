using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Mappers;

public static class RecipeMapper
{
  
       public  static List<RecipeResponseDto> ToResponseDtoList(List<Recipe?> recipes)
        {
            var recipeResponseDtos = new List<RecipeResponseDto>();
            foreach (var recipe in recipes)
            {
                recipeResponseDtos.Add(new RecipeResponseDto()
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Context = recipe.RecipeContext,
                });
            }
            return recipeResponseDtos;
        }

       public static RecipeResponseDto ToResponseDto(Recipe? recipe)
       {
           var dto = (new RecipeResponseDto()
           {
               Id = recipe.Id,
               Title = recipe.Title,
               Context = recipe.RecipeContext,
               Category = recipe.CategoryName
               
           });
           return dto;
       }

}