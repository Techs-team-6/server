using System.Text;
using Newtonsoft.Json;

namespace DedicatedMachine.Share;

public static class StreamExtension
{
    public static int ReadInt(this Stream stream)
    {
        return (stream.ReadByte() << 24) | (stream.ReadByte() << 16) | (stream.ReadByte() << 8) | (stream.ReadByte());
    }

    public static string ReadString(this Stream stream)
    {
        var size = ReadInt(stream);
        var bytes = new byte[size];
        
        var bytesRead = 0;
        while (bytesRead < size)
        {
            var read = stream.Read(bytes, bytesRead, size - bytesRead);
            if (read == -1)
                throw new Exception("Stream return EOF");
            bytesRead += read;
        }

        return Encoding.UTF8.GetString(bytes, 0, size);
    }

    public static void WriteInt(this Stream stream, int value)
    {
        stream.WriteByte((byte)(value >> 24));
        stream.WriteByte((byte)(value >> 16));
        stream.WriteByte((byte)(value >> 8));
        stream.WriteByte((byte)value);
    }
    
    public static void WriteString(this Stream stream, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteInt(stream, bytes.Length);
        stream.Write(bytes);
    }

    public static void WriteAction(this Stream stream, HubAction action)
    {
        WriteInt(stream, (int) action);
    }
    
    public static HubAction ReadHubAction(this Stream stream)
    {
        return (HubAction) stream.ReadInt();
    }
    
    public static void WriteAction(this Stream stream, ClientAction action)
    {
        WriteInt(stream, (int) action);
    }
    
    public static ClientAction ReadClientAction(this Stream stream)
    {
        return (ClientAction) stream.ReadInt();
    }
    
    public static void Write<T>(this Stream stream, T value)
    {
        WriteString(stream, JsonConvert.SerializeObject(value)) ;
    }
    
    public static T Read<T>(this Stream stream)
    {
        var str = ReadString(stream);
        return JsonConvert.DeserializeObject<T>(str);
    }
}