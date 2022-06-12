using Domain.Dto.DedicatedMachineDto;

namespace DMConnect.Share;

public interface IDedicatedMachineAgent
{
    void StartInstance(StartInstanceDto dto);
}