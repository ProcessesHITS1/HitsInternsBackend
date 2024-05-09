namespace svc_InterviewBack.Utils;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
