namespace Domain.Tools;

public class InvalidTokenException : ServiceException
{
    public InvalidTokenException(string message) : base(message)
    {
    }
}