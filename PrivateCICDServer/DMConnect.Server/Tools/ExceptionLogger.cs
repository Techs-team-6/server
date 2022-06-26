using Domain.Dto.DedicatedMachineDto;

namespace DMConnect.Server.Tools;

public class ExceptionLogger
{
    private ExceptionLoggerSettings _settings;
    
    public ExceptionLogger(ExceptionLoggerSettings settings)
    {
        _settings = settings;
    }

    public void Write(IDedicateMachineDto action)
    {
        var streamWriter = File.AppendText(_settings.LogPath);
        streamWriter.WriteLine(DtoFormat(action));
        streamWriter.Close();
    }

    private string DtoFormat(IDedicateMachineDto action)
    {
        return action switch
        {
            InstanceStdOutDto stdOutDto => $"InstanceId\t:\t{stdOutDto.InstanceId}\t,\tMessage\t:\t{stdOutDto.Message}\t.",
            InstanceStdErrDto stdErrDto => $"InstanceId\t:\t{stdErrDto.InstanceId}\t,\tMessage\t:\t{stdErrDto.Message}\t.",
            _ => string.Empty
        };
    }
}