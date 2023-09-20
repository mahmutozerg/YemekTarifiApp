using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Repositories;

public interface IRecipeRepository:IGenericRepository<Recipe>
{
     Task<List<Recipe>?> GetByCategoryNameAsync(string listName, string userId);


}