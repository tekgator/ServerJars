using ServerJarsAPI.Events;

namespace ServerJarsAPI.Extensions;

public static class StreamExtensions
{
    public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, long? contentLength, IProgress<ProgressEventArgs>? progress = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (!source.CanRead)
            throw new ArgumentException("Source stream must be readable", nameof(source));

        ArgumentNullException.ThrowIfNull(destination);

        if (!destination.CanWrite)
            throw new ArgumentException("Destination stream must be writable", nameof(destination));

        if (bufferSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(bufferSize));

        var buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        int bytesRead;
        while ((bytesRead = await source.ReadAsync(buffer, cancellationToken)) != 0)
        {
            await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            totalBytesRead += bytesRead;
            progress?.Report(new ProgressEventArgs() { BytesTransferred = totalBytesRead, TotalBytes = contentLength });
        }
    }
}