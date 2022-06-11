namespace Domain.Dto.DedicatedMachineDto;

public class AuthResultDto : IDedicateMachineDto
{
    public readonly bool IsSuccessful;
    public readonly Guid DedicatedMachineId;

    public Action Action => Action.AuthResult;
    
    public AuthResultDto(bool isSuccessful, Guid dedicatedMachineId)
    {
        IsSuccessful = isSuccessful;
        DedicatedMachineId = dedicatedMachineId;
    }
}