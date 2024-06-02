using System.Linq.Expressions;

namespace Interns.Chats.Domain
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public Chat RelatedChat { get; set; }
        public StoredFile File { get; set; }
        public string MimeType { get; set; }

        public static Expression<Func<Attachment, bool>> CanBeAccessed(Guid chatId, Guid fileId, Guid userId)
        {
            return attachment =>
                attachment.Id == fileId 
                && attachment.RelatedChat.Id == chatId
                && (attachment.RelatedChat.UserIds.Contains(userId) || attachment.RelatedChat.OwnerId == userId);
        }

        public static Expression<Func<Attachment, bool>> RelatedChatCanBeAccessed(Guid chatId, Guid userId)
        {
            return attachment => 
                attachment.RelatedChat.Id == chatId 
                && (attachment.RelatedChat.UserIds.Contains(userId) || attachment.RelatedChat.OwnerId == userId);
        }

    }
}
