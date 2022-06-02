using Domain.Dto.DedicatedMachineDto;
using Newtonsoft.Json;

namespace Server.Core.Test;

public class DmConnectSerializeTest
{
    public static void SerializeTest()
    {
        IDedicateMachineDto dto = new AuthDto(Guid.Empty, "asd");
        
        var str = JsonConvert.SerializeObject(dto);

        Console.WriteLine(str);
    }
}