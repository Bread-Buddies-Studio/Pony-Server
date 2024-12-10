// Directory //
namespace ServerSpace;
// Imports //
using System;
using System.Net;
using static Constants;
// Classes //
public static class NetworkData
{
    // Data //
    private static string hostName;
    private static IPHostEntry localHost;
    private static IPAddress ipAddress;
    private static IPEndPoint ipEndPoint;
    // Constructor //
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    static NetworkData()
    #pragma warning restore CS8618
    {
        Setup();
    }
    // Getters //
    public static string GetHostName() => hostName;
    public static IPHostEntry GetLocalHost() => localHost;
    public static IPAddress GetIPAddress() => ipAddress;
    public static IPEndPoint GetIPEndPoint() => ipEndPoint;
    // Setters //
   static async void Setup()
    {
        hostName = Dns.GetHostName();
        localHost = await Dns.GetHostEntryAsync(hostName);
        ipAddress = localHost.AddressList[0];
        ipEndPoint = new(ipAddress, portNum);
    }
    // Base Methods //
    static async Task Main(string[] args)
    {
        // Init Server //
        Console.WriteLine("Setting up Server!");
        var serverThread = Task.Run(() => Server.Setup());
        // Init Client //
        Console.WriteLine("Setting up Client");
        await Client.Setup();
        // Debug //
        Console.WriteLine("Finished?");
    }
}