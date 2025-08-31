// Directory //
namespace ServerSpace;
// Imports //
using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Text.Json;
using static NetworkData;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Nodes;
using System.Numerics;
using System.Collections;
using System.Drawing;
using Entities;
using ServerSpace.Services;
// Classes //
public static class Server2
{
    // Data //
    public static readonly Dictionary<IPAddress, Player> Clients = [];
    private static string hostName => GetHostName();
    private static IPHostEntry localHost => GetLocalHost();
    private static IPAddress ipAddress => GetIPAddress();
    private static IPEndPoint ipEndPoint => GetIPEndPoint();
    private static readonly TcpListener listener;
    // Constructor //
    static Server2()
    {
        listener = new(ipEndPoint);
    }
    // Base Methods //
    public static async Task Connect()
    {
        // Network Listener //
        TcpListener listener = new(ipEndPoint);
        // Json Serialization //
        JsonSerializerOptions jsonOptions = JsonSerializerOptions.Default;
        JsonTypeInfo outgoingJsonInfo = JsonTypeInfo.CreateJsonTypeInfo<Outgoing>(jsonOptions);
        JsonTypeInfo incomingJsonInfo = JsonTypeInfo.CreateJsonTypeInfo<Incoming>(jsonOptions);
        // 
        try
        {
            // Begin Listen //
            listener.Start();
            // Wait for Connection //
            using TcpClient handler = await listener.AcceptTcpClientAsync();
            // Get IP Adrress //
            IPEndPoint? ipep = (IPEndPoint?)handler.Client.RemoteEndPoint;
            IPAddress? ipa = ipep.Address;
            // Extract Stream //
            await using NetworkStream stream = handler.GetStream();
            // 
            Incoming clientInfo = (Incoming)await JsonSerializer.DeserializeAsync(stream, incomingJsonInfo);
            await JsonSerializer.SerializeAsync(stream, new Outgoing(), outgoingJsonInfo);
            // Sample output:
            //     Sent message: "📅 8/22/2022 9:07:17 AM 🕛"
        }
        finally // Cleanup //
        {
            listener.Stop();
        }
    }

    public static async void Accept(TcpClient client, JsonTypeInfo jsonInfo)
    {
        // Extract IP //
        IPAddress? ipAddress = ExtractIP(client);
        // Extract Stream //
        await using NetworkStream stream = client.GetStream();
        // Initialize Data //
        object? _incomingData = await JsonSerializer.DeserializeAsync(stream, jsonInfo);
        // Conditions //
        if (ipAddress is null)
            return;

        if (_incomingData is null)
            return;
        // Success //
        Incoming incomingData = (Incoming)_incomingData;
        // 
        UpdateClient(ipAddress, incomingData);
    }

    public static IPAddress? ExtractIP(TcpClient client)
    {
        IPEndPoint? ipep = (IPEndPoint?)client.Client.RemoteEndPoint;
        return ipep?.Address;
    }

    public static void UpdateClient(IPAddress ipAddress, Incoming data)
    {
        Clients.TryGetValue(ipAddress, out Player? player);
        // Checks //
        if (player is null)
        {
            // Settings //
            Clients[ipAddress] = new Player(new RectangleF());
            // Update Reference //
            player = Clients[ipAddress];
            // Invoke Event //
            PlayerJoined?.Invoke(Clients[ipAddress], EventArgs.Empty);
        }
        // Extract Data //
        string userName = data.UserName;
        string? message = data.Message;
        Vector2? position = data.Position;
        // Checks //
        if (message is not null)
            ChatService.AddMessage(userName, message);
        if (position is not null)
            player.MoveTo((Vector2)position);
    }
    // Events //
    public static event EventHandler<EventArgs>? PlayerJoined;
    // Event Methods //
    public static void OnPlayerJoined(object? sender, EventArgs _)
    {

    }
}

public class Outgoing
{
    public required List<Entity> Entities;
    public required List<string> ChatLog;
    // Constructor //
    [SetsRequiredMembers]
    public Outgoing()
    {
        Entities = Entity.Entities;
        ChatLog = ChatService.GetLog();
    }
    // Public Methods //
}

public class Incoming
{
    // Data //
    public required string UserName;
    public required string PassWord;
    public required string? Message;
    public required Vector2 Position;
    // Constructor //
}