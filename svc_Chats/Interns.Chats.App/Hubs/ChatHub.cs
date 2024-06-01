using Interns.Auth.Extensions;
using Interns.Chats.App.Dto;
using Interns.Chats.Domain;
using Interns.Chats.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Interns.Chats.App.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _dbContext;

        public ChatHub(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task OnConnectedAsync()
        {
            Guid currentUserId = Context?.User?.GetId() ?? throw new BadHttpRequestException("User has no id present");
            var chatIds = await _dbContext.Chats.Where(Chat.HasMember(currentUserId)).Select(x => x.Id).ToListAsync();

            if (chatIds.Count != 0)
            {
                foreach (var id in chatIds)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
                }
            }

            await base.OnConnectedAsync();
        }

        public async Task Send(string message, Guid chatId)
        {
            Guid currentUserId = Context?.User?.GetId() ?? throw new BadHttpRequestException("User has no id present");

            var group = await _dbContext.Chats.FirstAsync(Chat.CanBeAccessed(chatId, currentUserId));
            var msg = new Message { AuthorId = currentUserId, Content = message, SentAt = DateTime.UtcNow };
            group.Messages.Add(msg);

            await _dbContext.SaveChangesAsync();

            await Clients.Group(chatId.ToString())
                .SendAsync(
                    ChatHubConstants.SendMessageMethod, 
                    new MessageDto
                    {
                        Id = msg.Id,
                        ChatId = chatId,
                        Author = msg.AuthorId,
                        Message = msg.Content,
                        SentAt = msg.SentAt
                    });
        }

        public async Task Join(Guid chatId)
        {
            Guid currentUserId = Context?.User?.GetId() ?? throw new BadHttpRequestException("User has no id present");

            if (await _dbContext.Chats.AnyAsync(Chat.CanBeAccessed(chatId, currentUserId)))
            {
                throw new BadHttpRequestException("Group not found");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}
