namespace svc_InterviewBack.Utils;

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