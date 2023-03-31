using MediatR;
using UniDb.Persistence;
using UniDb.Persistence.Models;

namespace UniDb.Application.Mediators.Queries;

public class GetCourseByIdQuery:IRequest<Course>
{
    public long CourseId { get;  }

    public GetCourseByIdQuery(long id)
    {
        CourseId = id;
    }
}

public class GetCourseByIdQueryHandler:IRequestHandler<GetCourseByIdQuery, Course>
{
    private readonly UniversityDbContext _db;

    public GetCourseByIdQueryHandler(UniversityDbContext db)
    {
        _db = db;
    }

    public async Task<Course> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _db.Courses.FindAsync(request.CourseId);
        if (course == null)
        {
            throw new Exception(nameof(course) + "not Found");
        }

        return course;
    }
}