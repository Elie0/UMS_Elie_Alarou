using System.Reflection;
using System.Text.Json.Serialization;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniDb.Application.Mediators.Commands;
using UniDb.Domain.Services;
using UniDb.Persistence;
using UniDb.Persistence.Models;
using UniDb.WebApi;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient<IRequestHandler<CreateCourseCommand, Course>, CreateCourseCommandHandler>();
builder.Services.AddTransient<IValidator<CreateCourseCommand>, CreateCourseCommandValidator>();
builder.Services.AddScoped<IFirebaseCreateAccountService, CreateAccountService>();
builder.Services.AddScoped<IFirebaseLoginServer,FirebaseLoginServer>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new NpgsqlRangeDateOnlyConverter());
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




builder.Services.AddControllers().AddOData(opt => opt.Select().Filter());
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