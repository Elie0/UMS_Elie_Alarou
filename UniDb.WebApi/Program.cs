using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mapster;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UniDb.Application.Mediators.Commands;
using UniDb.Application.Mediators.Queries;
using UniDb.Infrastructure.Services;
using UniDb.Persistence;
using UniDb.Persistence.Models;
using UniDb.Common;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddOData(opt => opt.Select().Filter());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient<IRequestHandler<CreateCourseCommand, Course>, CreateCourseCommandHandler>();
builder.Services.AddTransient<IRequestHandler<GetCoursesQuery, List<CourseDto>>, GetCoursesQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetCourseByIdQuery, Course>, GetCourseByIdQueryHandler>();
builder.Services.AddTransient<IValidator<CreateCourseCommand>, CreateCourseCommandValidator>();
builder.Services.AddTransient<IRequestHandler<EnrollmentCommand,ClassEnrollment>,EnrollmentCommandHandler>();
builder.Services.AddTransient<IRequestHandler<TeacherCommand,TeacherPerCourse>,TeacherCommandHandler>();
builder.Services.AddScoped<IFirebaseCreateAccountService, CreateAccountService>();
builder.Services.AddScoped<IFirebaseLoginServer,FirebaseLoginServer>();
var coursesconfig = new TypeAdapterConfig();
coursesconfig.NewConfig<Course, CourseDto>()
    .Map(dest => dest.Id, src => src.Id)
    .Map(dest => dest.Name, src => src.Name)
    .Map(dest => dest.MaxStudentsNumber, src => src.MaxStudentsNumber)
    .Map(dest => dest.EnrolmentStartDate, src => src.EnrolmentDateRange.LowerBound)
    .Map(dest => dest.EnrolmentEndDate, src => src.EnrolmentDateRange.UpperBound);
   

builder.Services.AddSingleton(coursesconfig);


builder.Services.AddAutoMapper(typeof(AutoMapperProfile));



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateRangeOnlyConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/admin-98661";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/admin-98661",
            ValidateAudience = true,
            ValidAudience = "admin-98661",
            ValidateLifetime = true
        };
    });
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<UniversityDbContext>(opt => opt
        .UseNpgsql(builder.Configuration.GetConnectionString("UniversityDb")));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();