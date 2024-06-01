using Interns.Auth.Extensions;
using Interns.Chats.App.Dto;
using Interns.Chats.Domain;
using Interns.Chats.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Interns.Chats.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly ChatDbContext _dbContext;

        public GroupsController(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("direct")]
        public Task CreateDirect([FromBody] CreateDirectGroupDto dto)
            => Create(new CreateGroupDto { Name = dto.Name, Users = [dto.UserId] });

        [HttpPost]
        public async Task Create([FromBody] CreateGroupDto dto)
        {
            await _dbContext.Groups.AddAsync(new Group
            {
                Name = dto.Name,
                UserIds = dto.Users,
                OwnerId = User.GetId()
            });

            await _dbContext.SaveChangesAsync();
        }

        [HttpGet("my")]
        public Task<List<GroupDto>> GetMyGroups()
            => _dbContext.Groups
                    .Where(x => x.UserIds.Contains(User.GetId()))
                    .Select(x => new GroupDto { Name = x.Name, Id = x.Id })
                    .ToListAsync();

        [HttpPost("{groupId}/add/{userId}")]
        public async Task AddToGroup([FromRoute] Guid groupId, [FromRoute] Guid userId)
        {
            var group = await _dbContext.Groups.FirstAsync(x => x.Id == groupId && x.OwnerId == User.GetId());
            group.UserIds.Add(userId);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost("{groupId}/remove/{userId}")]
        public async Task RemoveFromGroup([FromRoute] Guid groupId, [FromRoute] Guid userId)
        {
            var group = await _dbContext.Groups.FirstAsync(x => x.Id == groupId && x.OwnerId == User.GetId());
            group.UserIds.Remove(userId);
            await _dbContext.SaveChangesAsync();
        }

        [HttpGet("{groupId}/messages")]
        public async Task<List<MessageDto>> GetGroupMessages([FromRoute] Guid groupId, [FromQuery] DateTime? from, [FromQuery] DateTime? until)
        {
            until ??= DateTime.UtcNow;
            from ??= until - TimeSpan.FromHours(1);

            Guid currentUserId = User.GetId();
            var messages = await _dbContext.Groups
                .Where(Group.CanBeAccessed(groupId, currentUserId))
                .SelectMany(x => x.Messages)
                .Where(x => x.SentAt >= from && x.SentAt <= until)
                .Select(x => new MessageDto
                {
                    Id = x.Id,
                    ChatId = groupId,
                    Author = x.AuthorId,
                    Message = x.Content,
                    SentAt = x.SentAt
                })
                .ToListAsync();

            return messages;
        }
    }
}
