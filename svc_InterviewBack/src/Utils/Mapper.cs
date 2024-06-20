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

        // TODO remove this
        CreateMap<SeasonDb, SeasonDetails>()
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => new Season
                {
                    Id = src.Id,
                    Year = src.Year,
                    SeasonStart = src.SeasonStart,
                    SeasonEnd = src.SeasonEnd,
                    IsClosed = src.IsClosed
                }));

        CreateMap<Company, CompanyInSeasonInfo>()
            .ForMember(dest => dest.NPositions, opt => opt.MapFrom(src => src.Positions.Count))
            .ForMember(dest => dest.SeasonYear, opt => opt.MapFrom(src => src.Season.Year));

        CreateMap<Position, PositionInfo>();

        CreateMap<Student, StudentInfo>()
            .ForMember(dest => dest.EmploymentStatus, opt => opt.MapFrom(src => src.EmploymentStatus.ToString()))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company != null ? src.Company.Id : (Guid?)null));

        CreateMap<Position, PositionInfo>();

        CreateMap<CompanyAndPosition, PositionInfo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Position.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Position.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Position.Description))
            .ForMember(dest => dest.NSeats, opt => opt.MapFrom(src => src.Position.NSeats))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company.Id))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.SeasonYear, opt => opt.MapFrom(src => src.Company.Season.Year));

        CreateMap<(Position, Student), InterviewRequest>()
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Item1))
            .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Item2));

        CreateMap<InterviewRequest, RequestDetails>()
            .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Position.Id))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id));

        CreateMap<PositionCreation, Position>();
        CreateMap<PositionUpdate, Position>();
        CreateMap<RequestResult,RequestResultData>();
        CreateMap<RequestResultUpdate,RequestResult>();
        CreateMap<RequestStatusTemplate,RequestStatusTemplateData>();
    }

}