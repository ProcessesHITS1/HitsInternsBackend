namespace Interns.Common
{
    public class NotFoundException(string message) : Exception(message)
    {
    }

    public class BadRequestException(string message) : Exception(message)
    {
    }

    public class AccessDeniedException(string message) : Exception(message)
    {
    }

    public class MicroserviceException(string message) : Exception(message)
    {
    }

    public class EmptyValueException(string message) : BadRequestException(message) { }
}
