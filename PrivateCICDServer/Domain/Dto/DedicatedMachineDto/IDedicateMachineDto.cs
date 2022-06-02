using Newtonsoft.Json;

namespace Domain.Dto.DedicatedMachineDto;

public interface IDedicateMachineDto
{
    [JsonIgnore]
    Action Action { get; }
}