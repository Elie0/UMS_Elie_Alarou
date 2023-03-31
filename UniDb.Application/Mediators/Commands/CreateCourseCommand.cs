using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using UniDb.Persistence;
using UniDb.Persistence.Models;

namespace UniDb.Application.Mediators.Commands;

public class CreateCourseCommand : IRequest<Course>
{
    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }
    
    public string? Tenant { get; set; }
    public NpgsqlRange<DateOnly> EnrolmentDateRange { get; set; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course>
{
    private readonly UniversityDbContext _database;

    private readonly ILogger<CreateCourseCommandHandler> _logger;
    //private readonly IMapper _mapper;

    public CreateCourseCommandHandler(UniversityDbContext context, ILogger<CreateCourseCommandHandler> logger)
    {
        _database = context;
        _logger = logger;
        // _mapper = mapper;
    }

    public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateCourseCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
      /*  var schemaName = "public";
        if (!string.IsNullOrEmpty(request.Tenant))
        {
            schemaName = $"tenant_{request.Tenant}";
        }
        _database.ChangeSchema(schemaName);*/

        var course = new Course 
        {
            Name = request.Name,
            MaxStudentsNumber = request.MaxStudentsNumber,
            EnrolmentDateRange = request.EnrolmentDateRange,
        };
        try
        {
            
            _database.Courses.Add(course);
            await _database.SaveChangesAsync(cancellationToken);
            return course;
        }
        catch (Exception e)
        {
            throw new BadRequestException("An error occurred while adding a new course: " + e);
        }
    }
}

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(35);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.MaxStudentsNumber).GreaterThan(0); 
        //RuleFor(x => x.EnrolmentDateRange).NotNull().Must(x => x.LowerBound <= x.UpperBound);
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}