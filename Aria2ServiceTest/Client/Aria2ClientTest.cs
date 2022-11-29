using Aria2Service.Client;
using Aria2Service.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aria2ServiceTest.Client;
[TestClass]
public class Aria2ClientTest
{
    public static Aria2Client A2Client { get; set; }
    public static Aria2RPCService A2Service { get; set; }

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        var conf = new ServerConf();
        A2Service = new Aria2RPCService(conf);
        A2Service.Start();
        Thread.Sleep(1000);
        A2Client = Aria2ClientGenerator.Generate(conf);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        A2Client.WsClient.Stop();
        A2Client.API.ShutdownAsync().Wait();
        // delete all downloaded file
        Directory.Delete(A2Service.Conf.Dir, true);
    }
    [TestMethod]
    public async Task LoadTask()
    {
        var _uri = "https://www.glitched.online/wp-content/uploads/2019/10/free-games-alan-wakef.jpg";
        var gid = await A2Client.API.AddUriAsync(new List<string> { _uri });

        var result = await A2Client.API.TellStatusAsync(gid);
        Console.WriteLine(result.Files.Count);
        Thread.Sleep(2000);
    }
}
