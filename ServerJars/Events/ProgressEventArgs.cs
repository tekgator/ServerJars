using System.Net.Http.Handlers;

namespace ServerJarsAPI.Events;

public class ProgressEventArgs : HttpProgressEventArgs
{
    public ProgressEventArgs(
        int progressPercentage,
        long bytesTransferred,
        long? totalBytes = null) : base(progressPercentage, string.Empty, bytesTransferred, totalBytes)
    {

    }
}