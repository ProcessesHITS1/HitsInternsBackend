using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

public interface IPositionService
{
    Task<PositionDetails> Create(Guid companyId, PositionData position);
}

public class PositionsService(InterviewDbContext context, IMapper mapper) : IPositionService
{
    public async Task<PositionDetails> Create(Guid companyId, PositionData position)
    {
        var company = await context.Companies.Include(c=>c.Positions).FirstOrDefaultAsync(c=>c.Id==companyId);
        if (company == null)
            throw new NotFoundException($"Company with Id: {companyId} wasn't found.");
        
        var positionEntity = mapper.Map<Position>(position);

        company.Positions.Add(positionEntity);
        await context.SaveChangesAsync();

        return mapper.Map<PositionDetails>(positionEntity);
    }
}