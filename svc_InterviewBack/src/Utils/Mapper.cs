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

        CreateMap<Company, CompanyInSeasonInfo>()//TODO:fix mapping  for Positions
            .ForMember(dest => dest.SeasonYear, opt => opt.MapFrom(src => src.Season.Year))
            .ForMember(dest => dest.NPositions, opt => opt.MapFrom(src => src.Positions.Sum(p => p.N)))
            .ForMember(dest => dest.Positions, opt => opt.MapFrom(src => src.Positions));
       
        CreateMap<Position, PositionInfo>()
            .ForMember(dest => dest.NPositions, opt => opt.MapFrom(src => src.N));
        
        CreateMap<Student, StudentInfo>()
            .ForMember(dest => dest.EmploymentStatus, opt => opt.MapFrom(src => src.EmploymentStatus.ToString()));
    }
    
}