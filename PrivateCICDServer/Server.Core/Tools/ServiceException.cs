namespace Server.Core.Tools;

public class ServiceException : Exception
{
    public ServiceException(string message) : base(message)
    {
    }
}