using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDb.Application.Mediators.Commands;
using UniDb.Infrastructure.Services;


namespace UniDb.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "teacher")]
[ApiExplorerSettings(IgnoreApi = false)]
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFirebaseLoginServer _login;

    public TeacherController(IMediator mediator,IFirebaseLoginServer login)
    {
        _mediator = mediator;
        _login = login;

    }
    [HttpPost("AssertCourse")]
    public async Task<IActionResult> TeacherAssertionCourse(TeacherCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync(string email, string password)
    {
        try
        {
            var token = await _login.LoginAsync(email, password);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
