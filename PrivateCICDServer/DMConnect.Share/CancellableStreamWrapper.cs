namespace DMConnect.Share;

public class CancellableStreamWrapper : Stream
{
    private readonly Stream _stream;
    private readonly CancellationToken _cancellationToken;

    public CancellableStreamWrapper(Stream stream, CancellationToken cancellationToken)
    {
        _stream = stream;
        _cancellationToken = cancellationToken;
    }

    public override void Flush()
    {
        _stream.FlushAsync(_cancellationToken).Wait(_cancellationToken);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _stream.ReadAsync(buffer, offset, count, _cancellationToken).Result;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _stream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _stream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _stream.WriteAsync(buffer, offset, count, _cancellationToken).Wait(_cancellationToken);
    }

    public override bool CanRead => _stream.CanRead;
    public override bool CanSeek => _stream.CanSeek;
    public override bool CanWrite => _stream.CanWrite;
    public override long Length => _stream.Length;

    public override long Position
    {
        get => _stream.Position;
        set => _stream.Position = value;
    }
}