using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

using SeasonDb = DAL.Season;
public interface IPositionService
{
    Task<PositionInfo> Create(SeasonDb season, Guid companyId, PositionData position);
    Task<List<PositionDetails>> Search(PositionQuery query, int page);
    Task<PositionInfo> Update(Guid positionId, PositionData positionData);
}

public class PositionsService(InterviewDbContext context, IMapper mapper) : IPositionService
{
    private const int PageSize = 10;
    public async Task<PositionInfo> Create(SeasonDb season, Guid companyId, PositionData position)
    {
        bool companyExistsInSeason = await context.Companies.AnyAsync(c => c.Id == companyId && c.Season.Id == season.Id);
        if (!companyExistsInSeason)
        {
            throw new NotFoundException($"Company {companyId} not found in season {season.Year}");
        }
        var positionEntity = mapper.Map<Position>(position);

        var company = await context.Companies
            .Include(c => c.Positions)
            .FirstAsync(c => c.Id == companyId);

        company.Positions.Add(positionEntity);
        await context.SaveChangesAsync();

        return mapper.Map<PositionInfo>(positionEntity);
    }

    public async Task<List<PositionDetails>> Search(PositionQuery query, int page)
    {
        var positions = await context.Companies
            // filter by company ids
            .Where(c => query.CompanyIds.IsNullOrEmpty() || query.CompanyIds.Contains(c.Id))
            .SelectMany(p => p.Positions, (c, p) => new { Company = c, Position = p })
            // filter by query
            .Where(cp => cp.Position.Title.Contains(query.Query) ||
            cp.Position.Description != null && cp.Position.Description.Contains(query.Query))
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        return positions.Select(cp => mapper.Map<PositionDetails>((cp.Company, cp.Position))).ToList();
    }

    public async Task<PositionInfo> Update(Guid positionId, PositionData positionData)
    {
        var positionEntity = await context.Positions.FindAsync(positionId);
        if (positionEntity == null) throw new NotFoundException($"Position {positionId} not found");

        if (positionData.Title != null)
        {
            positionEntity.Title = positionData.Title;
        }

        if (positionData.Description != null)
        {
            positionEntity.Description = positionData.Description;
        }

        if (positionData.NPositions.HasValue)
        {
            positionEntity.NPositions = positionData.NPositions.Value;
        }
        await context.SaveChangesAsync();
        return mapper.Map<PositionInfo>(positionEntity);
    }
    
}