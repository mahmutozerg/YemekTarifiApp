using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Services;

public interface IFavoriteService:IGenericService<Favorite>
{
    Task<CustomResponseNoDataDto> ToggleFavorites(string recipeId, string userId);
    Task<CustomResponseListDataDto<Favorite>> GetFavorites(string userId,int pageId);
    Task<CustomResponseNoDataDto>  DeleteUserFavorites(string userId);

}