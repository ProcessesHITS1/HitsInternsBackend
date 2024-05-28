using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

public interface IPositionService
{
    Task<PositionInfoResponse> Create(Guid companyId,PositionInfo position );
}

public class PositionsService(InterviewDbContext context) : IPositionService
{
    public async Task<PositionInfoResponse> Create(Guid companyId, PositionInfo position)
    {
        var company = await context.Companies.FindAsync(companyId);
        if (company == null)
            throw new NotFoundException($"Company with Id: {companyId} wasn't found.");


        var positionEntity = new Position
        {
            Company = company,
            Title = position.Title, 
            Description = position.Description,
            N = position.NPositions
        }; 

        await context.Positions.AddAsync(positionEntity);
        await context.SaveChangesAsync();

        return new PositionInfoResponse
        {
            Id = positionEntity.Id,
            Title = positionEntity.Title,
            Description = positionEntity.Description,
            NPositions = positionEntity.N,
            CompanyId = company.Id
        };
    }
}