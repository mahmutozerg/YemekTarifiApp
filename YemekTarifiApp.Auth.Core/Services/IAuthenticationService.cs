using YemekTarifiApp.Auth.Core.DTOs;

namespace YemekTarifiApp.Auth.Core.Services;

public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);

    Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

    Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

    Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLoginDto clientLoginDto);
    
}