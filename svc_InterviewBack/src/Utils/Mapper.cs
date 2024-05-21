using AutoMapper;
using svc_InterviewBack.Models;
using svc_InterviewBack.DAL;

namespace svc_InterviewBack.Utils;

using SeasonDb = DAL.Season;
using Season = Models.Season;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<SeasonDb, Season>();
        CreateMap<SeasonData, SeasonDb>();
        CreateMap<SeasonDb, SeasonDetails>()
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => new Season
                (src.Id, src.Year, src.SeasonStart, src.SeasonEnd)));

        CreateMap<Company, CompanyInSeasonInfo>()
            .ForMember(dest => dest.NPositions, opt => opt.MapFrom(src => ComputeNPositions(src)));

        CreateMap<Student, StudentInfo>()
            .ForMember(dest => dest.EmploymentStatus, opt => opt.MapFrom(src => ComputeEmploymentStatus(src)));
    }

    private static int ComputeNPositions(Company company)
    {
        return 0;
    }

    private static string ComputeEmploymentStatus(Student student)
    {
        return "Unemployed";
    }
}