// Directory //
namespace ServerSpace;
// Imports //
using System;
using System.Net;
using ServerSpace.Entities;
using ServerSpace.Services;
using static Constants;
// Classes //
public static class NetworkData
{
    // Data //
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private static string hostName;
    private static IPHostEntry localHost;
    private static IPAddress ipAddress;
    private static IPEndPoint ipEndPoint;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    // Getters //
    public static string GetHostName() => hostName;
    public static IPHostEntry GetLocalHost() => localHost;
    public static IPAddress GetIPAddress() => ipAddress;
    public static IPEndPoint GetIPEndPoint() => ipEndPoint;
    // Setters //
   static async Task Setup()
    {
        hostName = Dns.GetHostName();
        localHost = await Dns.GetHostEntryAsync(hostName);
        ipAddress = localHost.AddressList[0];
        ipEndPoint = new(ipAddress, portNum);
        // Display IP //
        Console.WriteLine(ipAddress);
    }
    // Base Methods //
    public static async Task Main(string[] args)
    {
        // Initialize Self! //
        await Setup();
        // Init Server //
        Console.WriteLine("Setting up Server!");
        await Server.Setup();
        // Debug //
        Console.WriteLine("Finished?");
    }
}