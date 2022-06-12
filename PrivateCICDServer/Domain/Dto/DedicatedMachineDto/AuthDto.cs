namespace Domain.Dto.DedicatedMachineDto;

public class AuthDto : IDedicateMachineDto
{
    public readonly Guid Id;
    public readonly string TokenString;

    public AuthDto(Guid id, string tokenString)
    {
        Id = id;
        TokenString = tokenString;
    }
}