// Directory //
namespace ServerSpace;
// Imports //
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static NetworkData;
// Classes //
public static class Server
{
    // Data //
    private static string hostName => GetHostName();
    private static IPHostEntry localHost => GetLocalHost();
    private static IPAddress ipAddress => GetIPAddress();
    private static IPEndPoint ipEndPoint => GetIPEndPoint();
    // Base Methods //
    public static async Task Setup()
    {
        // Initialize Listener //
        using Socket listener = new(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        // Setup Listener //
        listener.Bind(ipEndPoint);
        listener.Listen(100);
        // Handler //
        Socket handler = await listener.AcceptAsync();
        // Attempt Receive //
        while (true)
        {
            Console.WriteLine("Attempting Receive!");
            // Buffer //
            byte[] buffer = new byte[1_024];
            // Wait until receive //
            int received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            // Response //
            string response = Encoding.UTF8.GetString(buffer, 0, received);
            // Command EOM means acknowledgment required //
            string eom = "<|EOM|>";
            // Post-Conditions //
            if (!(response.IndexOf(eom) > -1))
                continue;
            // Acknowledge //
            Console.WriteLine(
                $"Socket server received message: \"{response.Replace(eom, "")}\"");
            // Acknowledge Command //
            string ackMessage = "<|ACK|>";
            // Echo Bytes //
            byte[] echoBytes = Encoding.UTF8.GetBytes(ackMessage);
            // Send Acknowledgement! // 
            await handler.SendAsync(echoBytes, 0);
            Console.WriteLine(
                $"Socket server sent acknowledgment: \"{ackMessage}\"");
            // Success //
            break;
            // Sample output:
            //    Socket server received message: "Hi friends 👋!"
            //    Socket server sent acknowledgment: "<|ACK|>"
        }
    }
}