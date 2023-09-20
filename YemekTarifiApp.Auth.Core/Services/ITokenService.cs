using YemekTarifiApp.Auth.Core.Configurations;
using YemekTarifiApp.Auth.Core.DTOs;
using YemekTarifiApp.Auth.Core.Models;

namespace YemekTarifiApp.Auth.Core.Services;

public interface ITokenService
{
    TokenDto CreateToken(User user);
    ClientTokenDto CreateTokenByClient(Client client);

}