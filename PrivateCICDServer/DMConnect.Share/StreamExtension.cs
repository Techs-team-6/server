using System.Text;
using Domain.Dto.DedicatedMachineDto;
using Newtonsoft.Json;
using Action = Domain.Dto.DedicatedMachineDto.Action;

namespace DMConnect.Share;

public static class StreamExtension
{
    public static T Read<T>(this Stream stream)
    {
        var str = ReadString(stream);
        return JsonConvert.DeserializeObject<T>(str)!;
    }

    public static void Write<T>(this Stream stream, T value)
    {
        if (value is IDedicateMachineDto dto)
            stream.WriteInt((int)dto.Action);
        stream.WriteString(JsonConvert.SerializeObject(value));
    }

    public static Action ReadAction(this Stream stream)
    {
        return (Action)stream.ReadInt();
    }

    private static int ReadInt(this Stream stream)
    {
        return (stream.ReadByte() << 24) | (stream.ReadByte() << 16) | (stream.ReadByte() << 8) | (stream.ReadByte());
    }

    private static void WriteInt(this Stream stream, int value)
    {
        stream.WriteByte((byte)(value >> 24));
        stream.WriteByte((byte)(value >> 16));
        stream.WriteByte((byte)(value >> 8));
        stream.WriteByte((byte)value);
    }

    private static string ReadString(this Stream stream)
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

    private static void WriteString(this Stream stream, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteInt(stream, bytes.Length);
        stream.Write(bytes);
    }
}