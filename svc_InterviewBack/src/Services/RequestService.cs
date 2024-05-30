using AutoMapper;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

public interface IRequestService
{
    Task<RequestDetails> CreateAsync(Guid studentId, Guid positionId);
}

public class RequestService(InterviewDbContext context, IMapper mapper) : IRequestService
{
    public async Task<RequestDetails> CreateAsync(Guid studentId, Guid positionId)
    {
        var student = await context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new NotFoundException($"Student was not found, id:{studentId}");
        }

        var position = await context.Positions
            .FirstOrDefaultAsync(p => p.Id == positionId);
        if (position == null) throw new NotFoundException($"Position was not found, id:{positionId}");
        
        var interviewRequest = mapper.Map<InterviewRequest>((position, student));

        context.InterviewRequests.Add(interviewRequest);
        await context.SaveChangesAsync();

        var interviewRequestDto = mapper.Map<RequestDetails>(interviewRequest);

        return interviewRequestDto;
    }
}