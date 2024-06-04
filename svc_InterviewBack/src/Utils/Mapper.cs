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
            .ForMember(dest => dest.SeasonYear, opt => opt.MapFrom(src => src.Season.Year));

        CreateMap<Position, PositionData>();

        CreateMap<Student, StudentInfo>()
            .ForMember(dest => dest.EmploymentStatus, opt => opt.MapFrom(src => src.EmploymentStatus.ToString()));

        CreateMap<Position, PositionInfo>();

        CreateMap<(Company, Position), PositionDetails>()
            .ForMember(dest => dest.CompanyInfo, opt => opt.MapFrom(src => src.Item1))
            .ForMember(dest => dest.PositionInfo, opt => opt.MapFrom(src => src.Item2));

        CreateMap<(Position, Student), InterviewRequest>()
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Item1))
            .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Item2));

        CreateMap<InterviewRequest, RequestDetails>()
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Position.Id))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id));

        CreateMap<PositionData, Position>();

    }

}