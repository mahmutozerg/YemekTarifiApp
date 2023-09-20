using Microsoft.EntityFrameworkCore;
using YemekTarifiApp.Core;
using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;
using YemekTarifiApp.Core.Repositories;
using YemekTarifiApp.Core.Services;
using YemekTarifiApp.Core.UnitOfWorks;

namespace YemekTarifiApp.Service.Services;

public class UserAppService:GenericService<User>,IUserAppService
{   
    private readonly IUserAppRepository _userAppRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecipeService _recipeService;

    public new async Task<CustomResponseNoDataDto> Remove(string id)
    {
        var entity = await _userAppRepository.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
        if (entity is null )
        {
            return CustomResponseNoDataDto.Fail(404,ResponseMessages.UserNotFound);
        }
        entity.UpdatedAt =DateTime.Now;
        entity.UpdatedBy = entity.Id;
        await _recipeService.DeleteUserRecipes(id);

        await _userAppRepository.RemoveAsync(entity);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }
    public async Task<CustomResponseNoDataDto> AddAsync(string id)
    {
        var entity = new User()
        {
            Id = id,
            Recipes = new List<Recipe>(),
            CreatedAt = DateTime.Now,
            CreatedBy = "AuthServer",
            


        };
        await _userAppRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        return CustomResponseNoDataDto.Success(200);
    }

    public UserAppService(IGenericRepository<User?> repository, IUnitOfWork unitOfWork, IUserAppRepository userAppRepository, IRecipeService recipeService) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userAppRepository = userAppRepository;
        _recipeService = recipeService;
    }
}