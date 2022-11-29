using Aria2NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aria2Service.Server;

public class DownloadTask : IDownloadTask
{
    public DownloadTask(DownloadStatusResult statusResult) { 
        Status= statusResult;
    }
    public float Percentage { get => Status.CompletedLength / Status.TotalLength; }
    public DownloadStatusResult Status { get; set; }
}

public class HttpFtpTask : DownloadTask
{
    public HttpFtpTask(DownloadStatusResult statusResult) : base (statusResult) {}
}

public class MetalinkTask : DownloadTask
{
    public MetalinkTask(DownloadStatusResult statusResult) : base(statusResult) { }
}

public class TorrentTask : DownloadTask
{
    public TorrentTask(DownloadStatusResult statusResult) : base(statusResult) { }
}