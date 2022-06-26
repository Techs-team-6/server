using System.Text.Json;
using Domain.Tools;

namespace Server.BlazorUI.Tools;

public class ErrorParser
{
    public static string GetErrorMessage(string description)
    {
        var responseIndex = description.Split("Response: ");

        if (responseIndex.Length <= 1) return description;
        var details = JsonSerializer.Deserialize<ErrorDetails>(responseIndex[1][..^1]);
        return details.Message;
    }
}