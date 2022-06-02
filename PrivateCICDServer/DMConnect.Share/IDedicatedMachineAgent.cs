using Domain.Dto.DedicatedMachineDto;

namespace DMConnect.Share;

public interface IDedicatedMachineAgent
{
    public void StartInstance(StartInstanceDto dto);
}