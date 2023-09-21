using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Mappers;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;
using YemekTarifiApp.Core.Services;
using YemekTarifiApp.Core.UnitOfWorks;

namespace YemekTarifiApp.Service.Services;

public class FavoriteService:GenericService<Favorite>,IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<User> _userRepository;
    public FavoriteService(IGenericRepository<Favorite?> repository, IUnitOfWork unitOfWork, IGenericRepository<User> userRepository, IFavoriteRepository favoriteRepository) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _favoriteRepository = favoriteRepository;
    }


    public async Task<CustomResponseNoDataDto> ToggleFavorites(string recipeId, string userId)
    {
        var user = await _userRepository.Where(u => u.Id == userId && !u.IsDeleted).FirstOrDefaultAsync();
        var favoriteExist= await _favoriteRepository.Where(f => f.RecipeId == recipeId && !f.IsDeleted).FirstOrDefaultAsync();
        if (favoriteExist != null)
            await _favoriteRepository.RemoveAsync(favoriteExist);

        
        if (user is null)
            return CustomResponseNoDataDto.Fail(404, ResponseMessages.UserNotFound);

        var favorite = new Favorite()
        {
            Id = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.Now,
            CreatedBy = userId,
            RecipeId = recipeId,
            User = user,
            UserId = userId
        };
        user.Favorites.Add(favorite);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(201);

    }



    public async Task<CustomResponseListDataDto<Favorite>> GetFavorites(string userId,int pageId)
    {
        var user = await _userRepository.Where(u => u.Id == userId && !u.IsDeleted).FirstOrDefaultAsync();
        if (user is null)
            CustomResponseListDataDto<Favorite>.Fail(ResponseMessages.UserNotFound, 404);
            
        
        return CustomResponseListDataDto<Favorite>.Success(await _favoriteRepository.GetFavorites(user,pageId),200);
    }

    public async Task<CustomResponseNoDataDto> DeleteUserFavorites(string userId)
    {
        var favorites =  _favoriteRepository.Where(f => f.UserId == userId && !f.IsDeleted);
        if (favorites == null)
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.RecipeNotFound);
        
        foreach (var favorite in favorites)
        {
            favorite.IsDeleted= true;
        }

        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }
}