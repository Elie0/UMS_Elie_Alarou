using AutoMapper;
using MediatR;
using UniDb.Persistence;
using UniDb.Persistence.Models;

namespace UniDb.Application.Mediators.Commands;

public class TeacherCommand:IRequest<TeacherPerCourse>
{
    public long TeacherId { get; set; }
    public long CourseId { get; set; }
}

public class TeacherCommandHandler : IRequestHandler<TeacherCommand, TeacherPerCourse>
{
    private readonly UniversityDbContext _db;
    private readonly IMapper _mapper;

    public TeacherCommandHandler(UniversityDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }public async Task<TeacherPerCourse> Handle(TeacherCommand request, CancellationToken cancellationToken)
    {
        var course = await _db.Courses.FindAsync(request.CourseId);
        if (course == null)
        {
            throw new Exception("Course not found.");
        }

        var assertion = new TeacherPerCourse()
        {
            CourseId = request.CourseId,
            TeacherId = request.TeacherId
        };
        _db.TeacherPerCourse.Add(assertion);

        await _db.SaveChangesAsync(cancellationToken);

        return assertion;
    }
}


