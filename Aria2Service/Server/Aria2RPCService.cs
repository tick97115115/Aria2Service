using Aria2NET;
using Aria2Service.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aria2Service.Server;

public class Aria2RPCService : Process, IAria2RPCService
{
    public Aria2RPCService(ServerConf conf) : base()
    {
        EnableRaisingEvents= true;
        StartInfo.CreateNoWindow= true;
        StartInfo.RedirectStandardError= true;
        StartInfo.RedirectStandardInput= true;
        StartInfo.RedirectStandardOutput= true;

        Conf = conf;
        if (OperatingSystem.IsWindows())
        {
            StartInfo.FileName = @"Resources\aria2-1.36.0-win-64bit-build1\aria2c.exe";
        } else
        {
            StartInfo.FileName = "aria2";
        }

        var argList = new string[] {
                $"--enable-rpc={Conf.EnableRpc.ToString().ToLower()}",
                $"--rpc-listen-port={Conf.RpcListenPort}",
                $"--save-session={Conf.SaveSession}",
                File.Exists(Conf.InputFile) ? 
                $"--input-file={Conf.InputFile}" : string.Empty,
                $"--dir={Conf.Dir}",
                $"--disk-cache={Conf.DiskCache}",
                $"--rpc-secret={Conf.RpcSecret}",
                $"--rpc-allow-origin-all={Conf.RpcAllowOriginAll.ToString().ToLower()}"
            }; //running will. paramters no problem.
        var arg = string.Join(" ", argList);
        StartInfo.Arguments = arg;
    }

    public ServerConf Conf { get; init; }

    public void ForceShutdown()
    {
        var _client = Aria2NetClientGenerator.Generate(Conf);
        _client.ForceShutdownAsync().Wait();
    }

    public void RunAria2Service()
    {
        Start();
    }

    public void SoftStop()
    {
        var _client = Aria2NetClientGenerator.Generate(Conf);
        _client.ShutdownAsync().Wait();
    }

    public async Task SoftStopAsync()
    {
        var _client = Aria2NetClientGenerator.Generate(Conf);
        await _client.ShutdownAsync();
    }
}
