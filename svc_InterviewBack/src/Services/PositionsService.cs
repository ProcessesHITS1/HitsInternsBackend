using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

public interface IPositionService
{
    Task<PositionDetailedInfo> Create(Guid companyId, PositionInfo position);
}

public class PositionsService(InterviewDbContext context, IMapper mapper) : IPositionService
{
    public async Task<PositionDetailedInfo> Create(Guid companyId, PositionInfo position)
    {
        var company = await context.Companies.FindAsync(companyId);
        if (company == null)
            throw new NotFoundException($"Company with Id: {companyId} wasn't found.");


        var positionEntity = mapper.Map<Position>(position);
        positionEntity.Company = company;

        context.Positions.Add(positionEntity);
        await context.SaveChangesAsync();

        return mapper.Map<PositionDetailedInfo>(positionEntity);
    }
}