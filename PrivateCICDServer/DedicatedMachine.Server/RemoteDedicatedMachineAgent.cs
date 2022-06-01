using System.Net.Sockets;
using DedicatedMachine.Share;

namespace DedicatedMachine.Server;

public class RemoteDedicatedMachineAgent : IDedicatedMachineAgent
{
    private readonly TcpClient _client;
    private readonly Stream _stream;
    private readonly Hub _hub;

    private Guid _id;

    public RemoteDedicatedMachineAgent(Hub hub, TcpClient client)
    {
        _hub = hub;
        _client = client;
        _stream = client.GetStream();
        new Thread(() => Serve()).Start();
    }

    private void Serve()
    {
        try
        {
            AuthOperation();

            while (true)
            {
                Console.WriteLine("Read action");
                var action = _stream.ReadHubAction();
                Console.WriteLine("Action: " + action);
                switch (action)
                {
                    case HubAction.Register:
                        Register();
                        break;
                    case HubAction.ConsoleWrite:
                    {
                        var dto = _stream.Read<StringDto>();
                        Console.Write(dto.Value);
                        // ConsoleWrite(dto.Value);
                        break;
                    }
                    case HubAction.ConsoleWriteLine:
                    {
                        var dto = _stream.Read<StringDto>();
                        Console.WriteLine(dto.Value);
                        // ConsoleWriteLine(dto.Value);
                        break;
                    }
                    default:
                    {
                        Console.Error.WriteLine("Unknown action : " + action);
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            Console.Error.WriteLine("Disconnecting");
        }
        finally
        {
            _client.Close();
        }
    }

    private void AuthOperation()
    {
        var action = _stream.ReadHubAction();
        if (action == HubAction.Register)
        {
            Register();
        }
        else if (action == HubAction.Authenthicate)
        {
            Authenthicate();
        }
        else
        {
            throw new Exception("Wrong credentials");
        }
    }

    private class StringDto
    {
        public string Value;
    }

    private class AuthDto
    {
        public Guid Id;
        public string TokenString;
    }

    private void Register()
    {
        var tokenStr = _stream.ReadString();
        var label = _stream.ReadString();
        var description = _stream.ReadString();
        _id = _hub.Register(tokenStr, label, description);
    }

    private void Authenthicate()
    {
        var dto = _stream.Read<AuthDto>();
        _id = _hub.Authenthicate(dto.Id, dto.TokenString);
    }

    public void ConsoleWrite(string value)
    {
        _stream.WriteAction(ClientAction.ConsoleWrite);
        _stream.Write(new StringDto(){Value = value});
    }

    public void ConsoleWriteLine(string value)
    {
        _stream.WriteAction(ClientAction.ConsoleWriteLine);
        _stream.Write(new StringDto(){Value = value});
    }
}