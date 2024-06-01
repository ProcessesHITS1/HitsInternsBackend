using System.Linq.Expressions;

namespace Interns.Chats.Domain
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        public List<Guid> UserIds { get; set; } = [];
        public List<Message> Messages { get; set; } = [];

        public static Expression<Func<Group, bool>> HasMember(Guid userId)
        {
            return g => g.UserIds.Contains(userId) || g.OwnerId == userId;
        }
        public static Expression<Func<Group, bool>> CanBeAccessed(Guid groupId, Guid userId)
        {
            return (group) => group.Id == groupId && (group.UserIds.Contains(userId) || group.OwnerId == userId);
        }
    }
}
