namespace Domain.Tools;

public class AuthException : ServiceException
{
    public AuthException(string message) : base(message)
    {
    }
    
    public AuthException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}