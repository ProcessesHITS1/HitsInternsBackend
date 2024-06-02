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
    public class ChatsController : ControllerBase
    {
        private readonly ChatDbContext _dbContext;

        public ChatsController(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public async Task<ChatDto> Create([FromBody] CreateChatDto dto)
        {
            var chat = new Chat
            {
                Name = dto.Name,
                UserIds = dto.Users,
                OwnerId = User.GetId()
            };
            _dbContext.Chats.Add(chat);

            await _dbContext.SaveChangesAsync();

            return new ChatDto
            {
                Id = chat.Id,
                Name = chat.Name
            };
        }

        [HttpPost("direct")]
        public Task<ChatDto> CreateDirect([FromBody] CreateDirectChatDto dto)
            => Create(new CreateChatDto { Name = dto.Name, Users = [dto.UserId] });

        [HttpGet("my")]
        public Task<List<ChatDto>> GetMyGroups()
            => _dbContext.Chats
                    .Where(x => x.UserIds.Contains(User.GetId()))
                    .Select(x => new ChatDto { Name = x.Name, Id = x.Id })
                    .ToListAsync();

        [HttpPost("{chatId}/add/{userId}")]
        public async Task AddToGroup([FromRoute] Guid chatId, [FromRoute] Guid userId)
        {
            var group = await _dbContext.Chats.FirstAsync(x => x.Id == chatId && x.OwnerId == User.GetId());

            if (group.UserIds.Contains(userId))
            {
                throw new BadHttpRequestException("User already in chat");
            }

            group.UserIds.Add(userId);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPost("{chatId}/remove/{userId}")]
        public async Task RemoveFromGroup([FromRoute] Guid chatId, [FromRoute] Guid userId)
        {
            var group = await _dbContext.Chats.FirstAsync(x => x.Id == chatId && x.OwnerId == User.GetId());
            group.UserIds.Remove(userId);
            await _dbContext.SaveChangesAsync();
        }

        [HttpGet("{chatId}/messages")]
        public async Task<List<MessageDto>> GetChatMessages([FromRoute] Guid chatId, [FromQuery] DateTime? from, [FromQuery] DateTime? until)
        {
            until ??= DateTime.UtcNow;
            from ??= until - TimeSpan.FromHours(1);

            Guid currentUserId = User.GetId();
            var messages = await _dbContext.Chats
                .Where(Chat.CanBeAccessed(chatId, currentUserId))
                .SelectMany(x => x.Messages)
                .Include(x => x.Attachments)
                .Where(x => x.SentAt >= from && x.SentAt <= until)
                .Select(x => new MessageDto
                {
                    Id = x.Id,
                    ChatId = chatId,
                    Author = x.AuthorId,
                    Message = x.Content,
                    SentAt = x.SentAt,
                    Attachments = x.Attachments.Select(y => new AttachmentDto { Id =  y.Id, MimeType = y.MimeType })
                })
                .ToListAsync();

            return messages;
        }

        [HttpPost("{chatId}/attachments")]
        public async Task<Guid> UploadAttachment([FromRoute] Guid chatId, IFormFile file)
        {
            Guid currentUserId = User.GetId();
            var chat = await _dbContext.Chats.FirstAsync(Chat.CanBeAccessed(chatId, currentUserId));

            using var binaryReader = new BinaryReader(file.OpenReadStream());
            byte[] fileData = binaryReader.ReadBytes((int)file.Length);

            var attachment = new Attachment
            {
                MimeType = file.ContentType,
                RelatedChat = chat,
                File = new StoredFile(fileData)
            };

            _dbContext.Attachments.Add(attachment);

            await _dbContext.SaveChangesAsync();

            return attachment.Id;
        }

        [HttpGet("{chatId}/attachments")]
        public async Task<List<AttachmentDto>> GetChatAttachmentsInfo([FromRoute] Guid chatId)
        {
            Guid currentUserId = User.GetId();

            return await _dbContext.Attachments
                .Where(Attachment.RelatedChatCanBeAccessed(chatId, currentUserId))
                .Select(x => new AttachmentDto
                {
                    Id = x.Id,
                    MimeType = x.MimeType
                })
                .ToListAsync();
        }

        [HttpGet("{chatId}/attachments/{fileId}")]
        public async Task<FileResult> GetAttachment([FromRoute] Guid chatId, [FromRoute] Guid fileId)
        {
            Guid currentUserId = User.GetId();
            var attachment = await _dbContext.Attachments
                .Include(x => x.File)
                .FirstAsync(Attachment.CanBeAccessed(chatId, fileId, currentUserId));

            var fileContent = attachment.File.Content;

            return File(fileContent, attachment.MimeType);
        }
    }
}
