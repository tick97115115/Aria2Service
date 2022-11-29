using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Aria2NET;
using Aria2Service.Server;
using WatsonWebsocket;

namespace Aria2Service.Client;

public class Aria2Client : IAria2Client
{
    public Aria2Client(ServerConf conf)
    {
        API = new Aria2NetClient($"http://127.0.0.1:{conf.RpcListenPort}/jsonrpc", conf.RpcSecret);
        WsClient = new WatsonWsClient($"ws://127.0.0.1:{conf.RpcListenPort}/jsonrpc", conf.RpcListenPort, false);
        WsClient.MessageReceived += OnMessageReceived;
        WsClient.ServerConnected += OnServerConnected;
        WsClient.ServerDisconnected += OnServerDisconnected;
    }

    public Aria2NetClient API { get; init; }
    public WatsonWsClient WsClient { get; init; }
    public HashSet<string> Downloading { get; set; }
    public HashSet<string> Suspended { get; set; }
    public Dictionary<string, Aria2Task> TaskMap { get; set; }

    public async Task LoadTask(ServerConf conf)
    {
        var allTasks = await API.TellAllAsync();
        allTasks.ToList().ForEach(item =>
        {
            Suspended.Add(item.Gid);
        });
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        var jsonData = JsonSerializer.Deserialize<JSON_NotificationData>(e.Data, new JsonSerializerOptions());
        if (jsonData != null)
        {
            switch (jsonData.Method)
            {
                case "aria2.onDownloadStart":
                    OnDownloadStart(jsonData);
                    break;
                case "aria2.onDownloadPause":
                    OnDownloadPause(jsonData);
                    break;
                case "aria2.onDownloadStop":
                    OnDownloadStop(jsonData);
                    break;
                case "aria2.onDownloadComplete":
                    OnDownloadComplete(jsonData);
                    break;
                case "aria2.onDownloadError":
                    OnDownloadError(jsonData);
                    break;
                case "aria2.onBtDownloadComplete":
                    OnBtDownloadComplete(jsonData);
                    break;
            }
        }
    }

    public void OnServerConnected(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnServerDisconnected(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDownloadStart(JSON_NotificationData data)
    {
        data.Params.ForEach(@params =>
        {
            Downloading.Add(@params.Gid);
        });
    }

    private void OnDownloadPause(JSON_NotificationData data)
    {
        data.Params.ForEach((@params) =>
        {
            if (Downloading.Contains(@params.Gid))
            {
                Downloading.Remove(@params.Gid);
            }
            Suspended.Add(@params.Gid);
        });
    }

    private void OnDownloadStop(JSON_NotificationData data)
    {
        data.Params.ForEach((@params) =>
        {
            if (Downloading.Contains(@params.Gid))
            {
                Downloading.Remove(@params.Gid);
            }
            Suspended.Add(@params.Gid);
        });
    }

    private void OnDownloadComplete(JSON_NotificationData data)
    {
        data.Params.ForEach(@params =>
        {
            Downloading.Remove(@params.Gid);
            Suspended.Remove(@params.Gid);
        });
    }

    private void OnDownloadError(JSON_NotificationData data)
    {
        data.Params.ForEach((@params) =>
        {
            Downloading.Remove(@params.Gid);
            Suspended.Add(@params.Gid);
        });
    }

    private void OnBtDownloadComplete(JSON_NotificationData data)
    {
        data.Params.ForEach(@params =>
        {
            Downloading.Remove(@params.Gid);
            Suspended.Remove(@params.Gid);
        });
    }

    public Task<Aria2Task> AddHttpFtpTaskAsync(string url)
    {
        throw new NotImplementedException();
    }

    public Task<Aria2Task> AddMetalinkTask(string metalink)
    {
        throw new NotImplementedException();
    }

    public Task<Aria2Task> AddTorrentTask(string path)
    {
        throw new NotImplementedException();
    }
}

public class Aria2ClientGenerator
{
    public static Aria2Client Generate(ServerConf conf)
    {
        return new Aria2Client(conf);
    }
}

public class Aria2NetClientGenerator
{
    public static Aria2NetClient Generate(ServerConf conf)
    {
        return new Aria2NetClient($"http://127.0.0.1:{conf.RpcListenPort}/jsonrpc", conf.RpcSecret);
    }
}

public class JSON_NotificationData
{
    [JsonInclude]
    public string Jsonrpc { get; set; }
    [JsonInclude]
    public string Method { get; set; }
    [JsonInclude]
    public List<JSON_TaskGidData> Params { get; set; }
}

public class JSON_TaskGidData
{
    public string Gid { get; set; }
}