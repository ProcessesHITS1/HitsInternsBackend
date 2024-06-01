namespace Interns.Chats.App.Dto
{
    public class CreateDirectGroupDto
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
    public class CreateGroupDto
    {
        public string Name { get; set; }
        public List<Guid> Users { get; set; }
    }
}