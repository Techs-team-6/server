using Domain.Dto.DedicatedMachineDto;

namespace DMConnect.Share;

public interface IDedicatedMachineHub
{
    void InstanceOutErr(InstanceStdOutDto dto);
    
    void InstanceStdErr(InstanceStdErrDto dto);
}