using YemekTarifiApp.Auth.Core.DTOs;

namespace YemekTarifiApp.Auth.Core.Services;

public interface IUserService
{
    Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    Task<Response<NoDataDto>> Remove(string id);
}