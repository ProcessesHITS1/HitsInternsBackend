namespace Interns.Chats.App.Dto
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid Author { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
