﻿using AutoMapper;
using Interns.Common;
using Interns.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils.Extensions;

namespace svc_InterviewBack.Services;

public interface IPositionService
{
    Task<PositionInfo> Create(PositionCreation position);
    Task Delete(Guid id);
    Task<PaginatedItems<PositionInfo>> Search(PositionQuery query, int page);
    Task<PositionInfo> Get(int year, Guid positionId);
    Task<PositionInfo> Update(Guid positionId, PositionUpdate positionData);
}

public class PositionsService(InterviewDbContext context, IMapper mapper) : IPositionService
{
    private const int PageSize = 10;
    public async Task<PositionInfo> Create(PositionCreation position)
    {
        var company = await context.Companies
            .Include(x => x.Positions)
            .Include(x => x.Season)
            .FirstOrDefaultAsync(x => x.Id == position.CompanyId && x.Season.Year == position.SeasonYear)
            ?? throw new NotFoundException($"Company {position.CompanyId} not found in season {position.SeasonYear}");
        var positionEntity = mapper.Map<Position>(position);

        company.Positions.Add(positionEntity);
        await context.SaveChangesAsync();
        return mapper.Map<PositionInfo>(new CompanyAndPosition(company, positionEntity));
    }

    public async Task Delete(Guid id)
    {
        var position = await context.Positions.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException($"Position with id {id} not found");

        context.Remove(position);
        await context.SaveChangesAsync();
    }

    public async Task<PositionInfo> Get(int year, Guid positionId)
    {
        var position = await context.Positions
            .Include(x => x.Company)
            .FirstOrDefaultAsync(p => p.Id == positionId && p.Company.Season.Year == year)

            ?? throw new NotFoundException("No such position");
        var res = mapper.Map<PositionInfo>(new CompanyAndPosition(position.Company, position));
        res.NRequests = await context.InterviewRequests.CountAsync(r => r.Position.Id == positionId);
        return res;
    }

    public async Task<PaginatedItems<PositionInfo>> Search(PositionQuery query, int page)
    {
        var allPosition = context.Companies
            // filter by company ids
            .Where(c => c.Season.Year == query.SeasonYear)
            .Where(c => query.CompanyIds.IsNullOrEmpty() || query.CompanyIds.Contains(c.Id))
            .SelectMany(p => p.Positions, (c, p) => new { Company = c, Position = p })
            // filter by query
            .Where(cp =>
                cp.Position.Title.Contains(query.Query)
                || cp.Position.Description != null && cp.Position.Description.Contains(query.Query)
            );

        return await allPosition.Paginated(
            page,
            PageSize,
            cp =>
            {
                var mapped = mapper.Map<PositionInfo>(new CompanyAndPosition(cp.Company, cp.Position));
                mapped.SeasonYear = query.SeasonYear;
                mapped.CompanyId = cp.Company.Id;

                return mapped;
            }
        );
    }

    public async Task<PositionInfo> Update(Guid positionId, PositionUpdate positionData)
    {
        var positionEntity = await context.Positions.FindAsync(positionId);
        if (positionEntity == null) throw new NotFoundException($"Position {positionId} not found");

        positionEntity.UpdateProperties(mapper.Map<Position>(positionData));

        await context.SaveChangesAsync();
        return mapper.Map<PositionInfo>(positionEntity);
    }

}