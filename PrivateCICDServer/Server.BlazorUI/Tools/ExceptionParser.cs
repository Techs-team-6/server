using System.Text.Json;
using Domain.Tools;
using Server.API.Client.Contracts;

namespace Server.BlazorUI.Tools;

public static class ExceptionParser
{
    public static string Parse(Exception? exception)
    {
        return exception switch
        {
            AggregateException aggregateException => Parse(aggregateException.InnerException),
            SwaggerException swaggerException =>
                JsonSerializer.Deserialize<ErrorDetails>(swaggerException.Message).Message,
            _ => exception?.Message ?? "No error message provided"
        };
    }
}