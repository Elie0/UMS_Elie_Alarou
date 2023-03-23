using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniDb.Application.Mediators.Commands;
using UniDb.Domain.Services;
using UniDb.Persistence.Models;

namespace UniDb.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFirebaseLoginServer _login;
    
    public AdminController(IMediator mediator,IFirebaseLoginServer login)
    {
        _mediator = mediator;
        _login = login;
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<Course>> CreateCourse(CreateCourseCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(CreateCourse), new { id = result.Id }, result);
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


