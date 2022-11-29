namespace Aria2Service.Server;

public class ServerConf
{
    public bool EnableRpc { get; set; } = true;
    public int RpcListenPort { get; set; } = 6800;
    public string Dir { get; set; } = Path.Combine( "Resources", "download");
    public string DiskCache { get; set; } = "0";
    public string RpcSecret { get; set; } = "MySecret123";
    public bool RpcAllowOriginAll { get; set; } = true;
    public string SaveSession { get; set; } = Path.Combine("Resources", "aria2-1.36.0-win-64bit-build1", "aria2.session");
    public string InputFile { get => SaveSession; }
}
