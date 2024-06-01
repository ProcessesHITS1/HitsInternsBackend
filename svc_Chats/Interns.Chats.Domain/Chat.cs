using System.Linq.Expressions;

namespace Interns.Chats.Domain
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        public List<Guid> UserIds { get; set; } = [];
        public List<Message> Messages { get; set; } = [];
        public static Expression<Func<Chat, bool>> HasMember(Guid userId)
        {
            return g => g.UserIds.Contains(userId) || g.OwnerId == userId;
        }
        public static Expression<Func<Chat, bool>> CanBeAccessed(Guid chatId, Guid userId)
        {
            return g => g.Id == chatId && (g.UserIds.Contains(userId) || g.OwnerId == userId);
        }
    }
}
