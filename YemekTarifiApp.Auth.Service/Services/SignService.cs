using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace YemekTarifiApp.Auth.Service.Services;

public class SignService
{
    public static SecurityKey GetSymmetricSecurityKey(string securityKey)
    {

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}