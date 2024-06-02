namespace Interns.Chats.Domain
{
    public class StoredFile
    {
        public Guid Id { get; set; }
        public byte[] Content { get; set; }

        public StoredFile() { }

        public StoredFile(byte[] file) 
        {
            Content = file;
        }
    }
}
