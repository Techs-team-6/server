using System.Text.RegularExpressions;
using Domain.Services;

namespace Server.Core.Services;

public class NameValidatorService : INameValidatorService
{
    private const string Pattern = @"[a-zA-Z0-9 \-_]+";

    public bool IsValidProjectName(string name)
    {
        return Regex.IsMatch(name, Pattern);
    }

    public bool IsValidInstanceName(string name)
    {
        return Regex.IsMatch(name, Pattern);
    }
}