using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemekTarifiApp.Core.Services;

namespace YemekTarifiApp.API.Controllers;

[Authorize]
public class UserController:CustomBaseController
{
    private readonly IUserAppService _userAppService;

    public UserController(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(string userId)
    {
        
        var createdUser =  await _userAppService.AddAsync(userId);
        return CreateActionResult(createdUser);

    }

    [HttpDelete]
    public async Task<IActionResult> Remove(string userId)
    {
        var response = await _userAppService.Remove(userId);

        return CreateActionResult(response);
    }
}