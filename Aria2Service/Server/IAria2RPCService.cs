using Aria2NET;
using Aria2Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonWebsocket;

namespace Aria2Service.Server;

public interface IAria2RPCService
{
    public ServerConf Conf { get; init; }
    public void RunAria2Service();
    public Task SoftStopAsync();
    public void SoftStop();
    public void ForceShutdown();
}

public interface ITaskSystem
{
    Task<HttpFtpTask> CreateHttpFtpTask(string url);
    Task<TorrentTask> CreateTorrentTask();
    Task<MetalinkTask> CreateMetelinkTask();
    //WatsonWsClient WsClient { get; init; }
    //Aria2NetClient API { get; init; }
    //HashSet<string> Downloading { get; set; }
    //HashSet<string> Suspended { get; set; }
    //Dictionary<string, Aria2Task> TaskMap { get; set; }
    //void OnMessageReceived(object sender, MessageReceivedEventArgs e);
    //void OnServerConnected(object sender, EventArgs e);
    //void OnServerDisconnected(object sender, EventArgs e);
}

