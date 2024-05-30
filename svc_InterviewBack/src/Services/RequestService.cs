using AutoMapper;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

public interface IRequestService
{
    Task<RequestDetailedInfo> CreateAsync(Guid studentId, Guid positionId);
}

public class RequestService(InterviewDbContext context, IMapper mapper) : IRequestService
{
    public async Task<RequestDetailedInfo> CreateAsync(Guid studentId, Guid positionId)
    {
        var student = await context.Students
            .Include(s => s.Season)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new NotFoundException($"Student was not found, id:{studentId}");
        }

        var season = student.Season;

        if (season == null)
        {
            throw new NotFoundException($"Student's season was not identified, season:{season}");
        }

        var position = await context.Positions
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == positionId);
        
        var interviewRequest = mapper.Map<InterviewRequest>((position, student));

        context.InterviewRequests.Add(interviewRequest);
        await context.SaveChangesAsync();

        var interviewRequestDto = mapper.Map<RequestDetailedInfo>(interviewRequest);

        return interviewRequestDto;
    }
}