using System.Buffers;

namespace DMConnect.Share;

internal class DisposableByteArray : IDisposable
{
    private int Size { get; }
    public byte[] Value { get; }

    public DisposableByteArray(int size)
    {
        Size = size;
        Value = ArrayPool<byte>.Shared.Rent(size);
    }

    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(Value);
    }
}