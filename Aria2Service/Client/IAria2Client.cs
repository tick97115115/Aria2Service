using Aria2NET;
using WatsonWebsocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aria2Service.Server;

namespace Aria2Service.Client;

public interface IAria2Client
{
    public Aria2NetClient API { get; init; }
    public WatsonWsClient WsClient { get; init; }
    public Task LoadTask(ServerConf conf); // Load task info
    HashSet<string> Downloading { get; set; }
    HashSet<string> Suspended { get; set; }
    Dictionary<string, Aria2Task> TaskMap { get; set; }
    void OnMessageReceived(object sender, MessageReceivedEventArgs e);
    void OnServerConnected(object sender, EventArgs e);
    void OnServerDisconnected(object sender, EventArgs e);
    Task<Aria2Task> AddHttpFtpTaskAsync(string url);
    Task<Aria2Task> AddMetalinkTask(string metalink);
    Task<Aria2Task> AddTorrentTask(string path);
}
