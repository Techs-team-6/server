using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;

namespace Domain.Services;

public interface IDedicatedMachineService
{
    DedicatedMachine RegisterMachine(RegisterDto dto);
    
    bool AuthMachine(AuthDto dto);

    void SetState(SetStateDto dto);
    
    List<DedicatedMachine> List();
}