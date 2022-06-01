namespace DedicatedMachine.Share.Dto;

public class AuthDto
{
    public readonly Guid Id;
    public readonly string TokenString;

    public AuthDto(Guid id, string tokenString)
    {
        Id = id;
        TokenString = tokenString;
    }
}