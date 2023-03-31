using AutoMapper;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UniDb.Persistence;
namespace UniDb.Application.Mediators.Queries;

public class GetCoursesQuery:IRequest<List<CourseDto>>
{
    
} 
public class CourseDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public int MaxStudentsNumber { get; set; }

    public DateTime? EnrolmentStartDate { get; set; }

    public DateTime? EnrolmentEndDate { get; set; }
}


public class GetCoursesQueryHandler:IRequestHandler<GetCoursesQuery,List<CourseDto>>
{
    private readonly UniversityDbContext _db;
    private readonly IMapper _mapper;

    public GetCoursesQueryHandler(UniversityDbContext db,IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _db.Courses.ToListAsync(cancellationToken: cancellationToken);
        return _mapper.Map<List<CourseDto>>(courses);
    }
}