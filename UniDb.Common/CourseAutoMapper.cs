using AutoMapper;
using UniDb.Application.Mediators.Queries;
using UniDb.Persistence.Models;

namespace UniDb.Common;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DateOnly, DateTime>()
            .ConstructUsing(d => d.ToDateTime(TimeOnly.MinValue));

        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.EnrolmentStartDate,
                opt => opt.MapFrom(src => src.EnrolmentDateRange.LowerBound))
            .ForMember(dest => dest.EnrolmentEndDate,
                opt => opt.MapFrom(src => src.EnrolmentDateRange.UpperBound));
    }
}