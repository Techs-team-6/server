using System.Collections.Immutable;
using System.Text;
using Domain.Dto.DedicatedMachineDto;
using Newtonsoft.Json;

namespace DMConnect.Share;

public static class StreamExtension
{
    public static IDedicateMachineDto ReadActionDto(this Stream stream)
    {
        var typeName = stream.ReadString();
        var type = ActionTypeByName[typeName];
        var json = ReadString(stream);
        return (IDedicateMachineDto)JsonConvert.DeserializeObject(json, type)!;
    }

    public static void WriteActionDto(this Stream stream, IDedicateMachineDto value)
    {
        stream.WriteString(value.GetType().Name);
        stream.WriteString(JsonConvert.SerializeObject(value));
    }

    private static int ReadInt(this Stream stream)
    {
        using var bytes = stream.ReadNBytes(4);
        return (bytes.Value[0] << 24) | (bytes.Value[1] << 16) | (bytes.Value[2] << 8) | bytes.Value[3];
    }

    private static void WriteInt(this Stream stream, int value)
    {
        var bytes = new[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
        stream.Write(bytes, 0, 4);
    }

    private static string ReadString(this Stream stream)
    {
        var size = stream.ReadInt();
        using var bytes = stream.ReadNBytes(size);
        return Encoding.UTF8.GetString(bytes.Value, 0, size);
    }

    private static void WriteString(this Stream stream, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        stream.WriteInt(bytes.Length);
        stream.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Reads exact bytes count
    /// </summary>
    /// <param name="stream">Source of data</param>
    /// <param name="length">Exact bytes count to read</param>
    /// <returns>Disposable byte array with data</returns>
    /// <exception cref="IOException"></exception>
    private static DisposableByteArray ReadNBytes(this Stream stream, int length)
    {
        var bytes = new DisposableByteArray(length);

        try
        {
            var bytesRead = 0;
            while (bytesRead < length)
            {
                var read = stream.Read(bytes.Value, bytesRead, length - bytesRead);
                if (read == -1)
                    throw new IOException($"Stream returned EOF. Expected {length} bytes, read {bytesRead}");
                bytesRead += read;
            }
        }
        catch (Exception)
        {
            bytes.Dispose();
            throw;
        }

        return bytes;
    }

    private static readonly ImmutableList<Type> ActionTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(IsDedicateMachineDto).ToImmutableList();

    private static readonly ImmutableDictionary<string, Type> ActionTypeByName =
        ActionTypes.ToImmutableDictionary(type => type.Name);

    private static bool IsDedicateMachineDto(Type? t)
    {
        return t is not null
               && typeof(IDedicateMachineDto).IsAssignableFrom(t)
               && !t.IsAbstract
               && !t.IsInterface;
    }
}