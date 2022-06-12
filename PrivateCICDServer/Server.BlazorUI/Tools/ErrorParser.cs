namespace Server.BlazorUI.Tools;

public class ErrorParser
{
    public static string GetErrorMessage(string description)
    {
        var responseIndex = description.IndexOf("Response: ", StringComparison.Ordinal);

        responseIndex += 12;
        var lastResponseSymbol = description.Length - 2;

        return description.Substring(responseIndex, lastResponseSymbol - responseIndex);
    }

    public static string GetErrorCode(string description)
    {
        var statusIndex = description.IndexOf("Status: ", StringComparison.Ordinal);

        statusIndex += 8;

        return description.Substring(statusIndex, 3);
    }
}