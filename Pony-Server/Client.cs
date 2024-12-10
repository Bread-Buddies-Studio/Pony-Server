// Directory //
namespace ServerSpace;
// Imports //
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

using static NetworkData;
// Classes //
public class Client
{
    // Data //
    private static string hostName => GetHostName();
    private static IPHostEntry localHost => GetLocalHost();
    private static IPAddress ipAddress => GetIPAddress();
    private static IPEndPoint ipEndPoint => GetIPEndPoint();
    // Base Methods //
    public static async Task Setup()
    {
        // Initialize Client //
        using Socket client = new(
            ipEndPoint.AddressFamily, 
            SocketType.Stream, 
            ProtocolType.Tcp);
        // Connect to Server //
        Console.WriteLine("Connecting!");
        await client.ConnectAsync(ipEndPoint);
        Console.WriteLine("Successfully Connected!");
        // Send Message //
        while (true)
        {
            // Encode Message //
            string message = "Hi friends 👋!<|EOM|>";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            // Send Message //
            await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine($"Socket client sent message: \"{message}\"");
            // Array to receive acknowledgement //
            byte[] buffer = new byte[1_024];
            int received = await client.ReceiveAsync(buffer, SocketFlags.None);
            // Response //
            string response = Encoding.UTF8.GetString(buffer, 0, received);
            // Success? //
            if (response == "<|ACK|>")
            {
                Console.WriteLine(
                    $"Socket client received acknowledgment: \"{response}\"");
                break;
            }
            // Sample output:
            //     Socket client sent message: "Hi friends 👋!<|EOM|>"
            //     Socket client received acknowledgment: "<|ACK|>"
        }
        // Shut Down! //
        client.Shutdown(SocketShutdown.Both);
    }
}