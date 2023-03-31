using Microsoft.AspNetCore.Mvc;
using UniDb.Infrastructure.Services;
namespace UniDb.WebApi.Controllers;
public class AuthController : ControllerBase
{
    private readonly IFirebaseCreateAccountService _createAccountService;

    public AuthController(IFirebaseCreateAccountService createAccountService)
    {
        _createAccountService = createAccountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string email, string password,string role,string name)
    {
        var result = await _createAccountService.RegisterAsync(email, password, role,name);

        if (result == "Account Succesfully Created")
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }
    
}