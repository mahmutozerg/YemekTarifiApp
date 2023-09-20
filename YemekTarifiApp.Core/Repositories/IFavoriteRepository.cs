using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Repositories;

public interface IFavoriteRepository:IGenericRepository<Favorite>
{
    Task<List<Favorite?>> GetFavorites(User user,int pageId);
}