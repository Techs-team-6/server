namespace Domain.Services;

public interface INameValidatorService
{
    bool IsValidProjectName(string name);

    bool IsValidInstanceName(string name);
}