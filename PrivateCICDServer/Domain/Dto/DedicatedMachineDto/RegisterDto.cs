namespace Domain.Dto.DedicatedMachineDto;

public class RegisterDto : IDedicateMachineDto
{
    public readonly string TokenString;
    public readonly string Label;
    public readonly string Description;

    public RegisterDto(string tokenString, string label, string description)
    {
        TokenString = tokenString;
        Label = label;
        Description = description;
    }
}