using Domain.Dto.DedicatedMachineDto;

namespace DMConnect.Share;

public interface IDedicatedMachineHub
{
    void InstanceStdOut(InstanceStdOutDto dto);
    
    void InstanceStdErr(InstanceStdErrDto dto);

    void InstanceSetState(InstanceSetStateDto dto);
}