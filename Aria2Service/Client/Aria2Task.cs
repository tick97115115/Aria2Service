using Aria2NET;

namespace Aria2Service.Client;

public class Aria2Task : IAria2Task
{
    public Aria2Task(ref Aria2NetClient api, string gid) 
    {
        API = api;
        Gid= gid;
    }
    public Aria2NetClient API {get; init; }
    public string Gid { get; init; }
    public DownloadStatusResult Status { get; set; }
    public float DownloadPercentage { get; set; }
    DownloadStatusResult IAria2Task.Status {get; set; }

    public async Task GetStatus()
    {
        Status = await API.TellStatusAsync(Gid);
        DownloadPercentage = Status.CompletedLength / Status.TotalLength;
    }

    public virtual async Task DeleteAsync()
    {
        throw new NotImplementedException();
    }

    public virtual async Task PauseAsync()
    {
        await API.PauseAsync(Gid);
    }

    public virtual async Task UnpauseAsync()
    {
        await API.UnpauseAsync(Gid);
    }
}

public class HttpFtpTask : Aria2Task
{
    public HttpFtpTask(ref Aria2NetClient api, string gid) : base (ref api, gid)
    {
        //
    }
    public override async Task DeleteAsync()
    {
        await API.RemoveAsync(Gid);
        await API.RemoveDownloadResultAsync(Gid);
        Status.Files.ForEach(f =>
        {
            File.Delete(f.Path);
        });
    }
}