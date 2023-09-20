using Microsoft.AspNetCore.Mvc;
using YemekTarifiApp.Auth.Core.DTOs;

namespace YemekTarifiApp.Auth.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CustomControllerBase:ControllerBase 
{
    [NonAction]
    public IActionResult CreateActionResult<T>(Response<T> res) where T : class
    {

        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }


}