using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using LiteNetLib;
using LiteNetLib.Utils;
using Network.Enums;
using Network.Services;
using Rendering.Entities;
using Rendering.Services;
using static LiteNetLib.EventBasedNetListener;

namespace Network;

public class Program : INetEventListener
{
    public static Program Singleton { get; private set; }
    public static readonly Dictionary<NetPeer, string> Clients = [];
    public static NetManager Server { get; private set; }
    // Events //
    public static event EventHandler<(NetPeer, NetPacketReader, byte, DeliveryMethod)>? NetworkReceive;
    // Constructors //
    [SetsRequiredMembers]
    static Program()
    {
        Singleton = new Program();
        Server = new NetManager(Singleton);
        Server.Start(PortNum);
        // Event Loop //
        Program.NetworkReceive += PacketService.OnNetworkReceive;
        // Server Loop //
        while (true)
        {
            Server.PollEvents();
            RunService.InvokeUpdate();
            Thread.Sleep(1000 / FPS);
        }
    }
    public static void Main() 
    {
    }
    
    [SetsRequiredMembers]
    public Program() : base()
    {
        // Connections //
        RunService.Update += OnUpdate;
        // Server Loop //
    }
    #region Event Methods
    void OnUpdate(object? sender, UpdateEventArgs eventData)
    {
        //foreach ((NetPeer client, Player player) in EntityService.Players)
        //{
        //    // Get Other Players //
        //    Player[] otherPlayers = [.. EntityService.Players.Values
        //        /*.Where(otherPlayer => player != otherPlayer)*/];
        //    // Data Writer //
        //    NetDataWriter writer = new();
        //    writer.PutArray(otherPlayers);
        //    // Send Data //
        //    client.Send(writer.Data, DeliveryMethod.ReliableOrdered);
        //}
    }
    #region Network Methods
    void INetEventListener.OnConnectionRequest(ConnectionRequest request)
    {
        // Debug //
        Console.WriteLine("Connection Requested!");
        // Sub-Methods //
        void Reject(ConnectionFail failReason)
        {
            NetDataWriter writer = new();
            writer.Put((int)failReason);
            request.Reject(writer);
        }
        // Extract Data //
        API connectionType = (API)request.Data.GetInt();
        string accountData = request.Data.GetString();
        Console.WriteLine(connectionType);
        Console.WriteLine(accountData + "\n");
        // Conditions //
        if (!byte.TryParse(accountData.AsSpan(0, 3), out byte tagLine))
        { 
            Reject(ConnectionFail.InvalidTagLine);
            return;
        }
        // Checks //
        if (connectionType is API.LogIn)
        {
            try
            {
                AccountService.LogIn(accountData);
            }
            catch (InvalidTaglineException)
            {
                Reject(ConnectionFail.InvalidTagLine);
                return;
            }
            catch (AccountNotFoundException)
            {
                Reject(ConnectionFail.AccountNotFound);
                return;
            }
        }
        else if (connectionType is API.SignUp)
        {
            try
            {
                AccountService.SignUp(accountData);
            }
            catch (InvalidTaglineException)
            {
                Reject(ConnectionFail.InvalidTagLine);
                return;
            }
            catch (AccountExistsException)
            {
                Reject(ConnectionFail.AccountAlreadyExists);
                return;
            }
        }
        // Success //
        Clients.Add(request.Accept(), accountData);
    }

    void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        throw new NotImplementedException();
    }

    void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        NetworkReceive?.Invoke(this, (peer, reader, channelNumber, deliveryMethod));
    }

    void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        throw new NotImplementedException();
    }

    void INetEventListener.OnPeerConnected(NetPeer peer)
    {
        EntityService.Add(peer, Clients[peer].Split(DataSeperator)[1]);
    }

    void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Clients.Remove(peer);
    }
    #endregion
    #endregion
}
