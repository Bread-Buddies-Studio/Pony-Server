using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib.Utils;
using LiteNetLib;
using Network.Enums;
using Avalonia;
using Rendering.Services;
using Core.Extensions;
using Rendering.Entities;
using System.Reflection.PortableExecutable;
using Avalonia.Controls.Shapes;

namespace Network.Services;

public static class PacketService
{
    static PacketService()
    {
    }

    internal static void SendToOtherClients(NetPeer clientException, byte[] data, DeliveryMethod deliveryMethod)
    {
        foreach (NetPeer otherClient in Program.Clients.Keys)
        {
            // Conditions //
            if (otherClient == clientException)
                continue;
            // Send //
            otherClient.Send(data, deliveryMethod);
        }
    }
    internal static void SendToOtherClients(NetPeer clientException, NetDataWriter writer, DeliveryMethod deliveryMethod)
    {
        foreach (NetPeer otherClient in Program.Clients.Keys)
        {
            // Conditions //
            if (otherClient == clientException)
                continue;
            // Send //
            otherClient.Send(writer, deliveryMethod);
        }
    }
    public static void OnNetworkReceive(object? sender, 
        (NetPeer client, 
        NetPacketReader reader, 
        byte channel, 
        DeliveryMethod deliveryMethod) eventData)
    {
        // Extract Data //
        PacketType packetType = (PacketType)eventData.reader.GetByte();
        string accountData = Program.Clients[eventData.client];
        // Check Packet Type //
        if (packetType is PacketType.PlayerUpdate)
        {// Find Player //
            Player clientPlayer = EntityService.Players[eventData.client];
            // Update Stored Player //
            eventData.reader.Get(() => clientPlayer);
            // Create Writer //
            NetDataWriter writer = new(); 
            writer.Put((byte)PacketType.PlayerUpdate);
            writer.Put(accountData[..accountData.LastIndexOf(DataSeperator)]);
            writer.Put(clientPlayer);
            // Sending Back Data! //
            SendToOtherClients(eventData.client, writer, DeliveryMethod.ReliableOrdered);
        }

        else if (packetType is PacketType.ChatMessage)
        {
            // Initialize Data Writer //
            NetDataWriter writer = new();
            // Data Extraction //
            Player clientPlayer = EntityService.Players[eventData.client];
            string chatMessage = eventData.reader.GetString();
            // Pack Data //
            writer.Put((byte)PacketType.ChatMessage);
            writer.Put(accountData[..accountData.LastIndexOf(DataSeperator)]);
            writer.Put(chatMessage);
            // Debug //
            Console.WriteLine($"{clientPlayer.Name}: {chatMessage}");
            // Update //
            ChatService.AddLog(eventData.client, chatMessage);
            // Send //
            SendToOtherClients(eventData.client, writer, eventData.deliveryMethod);
        }
    }
}
