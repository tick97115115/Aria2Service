using Aria2NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace Aria2Service.Server;

public interface IDownloadTask
{
    public DownloadStatusResult Status { get; set; }
    public  float Percentage { get => Status.CompletedLength / Status.TotalLength; }
}
