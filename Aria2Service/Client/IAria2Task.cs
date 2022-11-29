using Aria2NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aria2Service.Client;

public interface IAria2Task
{
    Aria2NetClient API { get; init; }
    string Gid { get; init; }
    float DownloadPercentage { get; set; }
    DownloadStatusResult Status { get; set; }
    Task GetStatus();
    Task DeleteAsync();
    Task PauseAsync();
    Task UnpauseAsync();
}
