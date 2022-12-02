namespace ServerJarsAPI.Events;

public class ProgressEventArgs
{
    public long BytesTransferred { get; set; } = 0;

    public long? TotalBytes { get; set; } = null;

    public double ProgressPercentage
    {
        get
        {
            if (!TotalBytes.HasValue || TotalBytes <= 0)
            {
                return 0;
            }

            if (BytesTransferred == 0)
            {
                return 0;
            }

            return Math.Round((double)(BytesTransferred * 100 / TotalBytes), 1);
        }
    }
}