using Aria2Service.Server;

namespace Aria2ServiceTest.Server;

[TestClass]
public class ClassName
{
    public static Aria2RPCService Service;
    [TestInitialize]
    public void TestInitialize()
    {
        Service = new Aria2RPCService(new ServerConf());
    }


    [TestCleanup]
    public void TestCleanup()
    {
        if (!Service.HasExited)
        {
            Service.Kill();
            Service.WaitForExit();
            Service.Close();
        }
    }

    [TestMethod]
    public void Start()
    {
        Service.RunAria2Service();
        Thread.Sleep(1000);
        Assert.IsFalse(Service.HasExited);
    }

    [TestMethod]
    public void SoftStop_default()
    {
        Service.RunAria2Service();
        Thread.Sleep(1000);
        Service.SoftStop(); //SoftStop takes more than 2 seconds.
        Thread.Sleep(10000);
        Assert.IsTrue(Service.HasExited);
    }
}