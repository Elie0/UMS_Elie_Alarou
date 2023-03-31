using MediatR;
using UniDb.Persistence;
using UniDb.Persistence.Models;
using System.Text.Json;
using AutoMapper;
using UniDb.Application.Mediators.Queries;


namespace UniDb.Application.Mediators.Commands;

public class EnrollmentCommand:IRequest<ClassEnrollment>
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

public class EnrollmentCommandHandler : IRequestHandler<EnrollmentCommand, ClassEnrollment>
{
    private readonly UniversityDbContext _db;
    private readonly IMapper _mapper;

    public EnrollmentCommandHandler(UniversityDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ClassEnrollment> Handle(EnrollmentCommand request, CancellationToken cancellationToken)
    {
        var course = await _db.Courses.FindAsync(request.CourseId);
        if (course == null)
        {
            throw new Exception("Course not found.");
        }

        // Map the EnrolmentDateRange to DateRangeDto
        var courseDto = _mapper.Map<CourseDto>(course);
        var lowerBound = courseDto.EnrolmentStartDate;
        var upperBound = courseDto.EnrolmentEndDate;

        if (!(request.EnrollmentDate >= lowerBound && request.EnrollmentDate <= upperBound))
        {
            throw new Exception("Enrollment date is not within the allowed range.");
        }

        var enrollment = new ClassEnrollment
        {
            ClassId = request.CourseId,
            StudentId = request.StudentId
        };
        _db.ClassEnrollment.Add(enrollment);

        await _db.SaveChangesAsync(cancellationToken);

        return enrollment;
    }
}
