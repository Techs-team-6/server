namespace DedicatedMachine.Share;

public interface IDedicatedMachineHub
{
    // void Register(string tokenStr, string label, string description);
    
    // void Authenthicate(Guid id, string tokenStr);
    
    void ConsoleWrite(string value);
    
    void ConsoleWriteLine(string value);
}