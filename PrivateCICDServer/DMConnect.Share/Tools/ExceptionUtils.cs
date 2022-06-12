namespace DMConnect.Share.Tools;

public static class ExceptionUtils
{
    public static bool IsOperationCanceled(Exception e)
    {
        if (e is AggregateException { InnerException: { } } aggregateException)
        {
            e = aggregateException.InnerException;
        }

        return e is OperationCanceledException;
    }
}