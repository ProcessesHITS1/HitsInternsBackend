using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

public interface IRequestService
{
    Task<InterviewRequest> CreateAsync(Guid studentId, Guid positionId);
}

public class RequestService(InterviewDbContext context) : IRequestService
{
    public async Task<InterviewRequest> CreateAsync(Guid studentId, Guid positionId)
    {
        var student = await context.Students
            .Include(s => s.Season)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new NotFoundException("Student was not found");
        }
        var season = student.Season;
        
        if (season == null || season.Id == Guid.Empty)
        {
            throw new NotFoundException("Student's season was not identified");
        }

        var position = await context.Positions
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == positionId && p.Company.Season.Id == season.Id);

        if (position?.Company == null)
        {
            throw new NotFoundException("Position or company was not found in the student's season");
        }

        var interviewRequest = new InterviewRequest
        {
            Position = position,
            Student = student,
            Status = RequestStatus.Pending
        };

        await context.InterviewRequests.AddAsync(interviewRequest);
        await context.SaveChangesAsync();
        return interviewRequest;
    }
}

