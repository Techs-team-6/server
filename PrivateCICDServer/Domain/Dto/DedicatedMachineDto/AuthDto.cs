namespace Domain.Dto.DedicatedMachineDto;

public class AuthDto : IDedicateMachineDto
{
    public readonly Guid Id;
    public readonly string TokenString;
    
    public Action Action => Action.Authenthicate;

    public AuthDto(Guid id, string tokenString)
    {
        Id = id;
        TokenString = tokenString;
    }
}