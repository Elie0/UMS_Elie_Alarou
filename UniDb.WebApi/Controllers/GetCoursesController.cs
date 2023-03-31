using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using UniDb.Application.Mediators.Queries;
using UniDb.Persistence.Models;

namespace UniDb.WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GetCoursesController:ControllerBase
{
    private readonly IMediator _mediator;

    public GetCoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<List<CourseDto>> GetCourses()
    {
        var query = new GetCoursesQuery();
        var result = await _mediator.Send(query);
        return result;
    }
    
    [HttpGet("byId")]
    public async Task<Course> GetCourseById(long id)
    {
        var query = new GetCourseByIdQuery(id);
        var result = await _mediator.Send(query);
        return result;
    }
}
