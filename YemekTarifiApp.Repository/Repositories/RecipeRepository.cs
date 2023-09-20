using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;

namespace YemekTarifiApp.Repository.Repositories;

public class RecipeRepository:GenericRepository<Recipe>,IRecipeRepository
{
    private readonly DbSet<Recipe> _dbSet;

    public RecipeRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<Recipe>();
    }

    public async Task<List<Recipe>?> GetByCategoryNameAsync(string categoryName, string userId)
    {
        
        return await _dbSet.Where(t => t.CategoryName == categoryName && t.UserId == userId && !t.IsDeleted)
               .AsNoTracking().ToListAsync();
         
    }

    
}