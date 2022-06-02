namespace Domain.Services;

public interface INameValidatorService
{
    bool IsValidProjectName(string name);
}