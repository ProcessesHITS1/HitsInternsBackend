namespace Interns.Chats.Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public List<Attachment> Attachments { get; set; } = [];
    }
}
