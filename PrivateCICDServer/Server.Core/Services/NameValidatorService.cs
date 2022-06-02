using System.Text.RegularExpressions;
using Domain.Services;

namespace Server.Core.Services;

public class NameValidatorService : INameValidatorService
{
    private string _pattern = @"[a-zA-Z0-9 \-_]+";
    public bool IsValidProjectName(string name)
    {
        return Regex.IsMatch(name, _pattern);
    }
}