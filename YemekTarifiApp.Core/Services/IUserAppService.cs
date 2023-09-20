using YemekTarifiApp.Core.DTOs;
using YemekTarifiApp.Core.Models;

namespace YemekTarifiApp.Core.Services;

public interface IUserAppService:IGenericService<User>
{
    Task<CustomResponseNoDataDto> AddAsync(string id);
    Task<CustomResponseNoDataDto> Remove(string id);


    public Task<bool> Update(User? entity);

}