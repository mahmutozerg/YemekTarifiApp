using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;

namespace YemekTarifiApp.Repository.Repositories;

public class FavoriteRepository:GenericRepository<Favorite>,IFavoriteRepository
{
    private readonly DbSet<Favorite> _dbSet;

    public FavoriteRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<Favorite>();
    }

    public async Task<List<Favorite?>> GetFavorites(User user,int pageId)
    {
       return await _dbSet.Where(f=> f.UserId == user.Id).OrderBy(x => x.CreatedBy).Skip((pageId-1)*10).Take(10).ToListAsync();
    }
}