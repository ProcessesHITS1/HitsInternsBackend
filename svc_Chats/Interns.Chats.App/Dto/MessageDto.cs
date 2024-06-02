namespace Interns.Chats.App.Dto
{
    public class PostMessageDto
    {
        public string Message { get; set; }
        public Guid ChatId { get; set; }
        public List<Guid> AttachmentIds { get; set; } = [];
    }
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid Author { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public IEnumerable<AttachmentDto> Attachments { get; set; } = [];
    }
}
