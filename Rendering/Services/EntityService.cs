using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using Rendering.Entities;

namespace Rendering.Services;

public static class EntityService
{
    // Data //
    public static readonly Dictionary<NetPeer, Player> Players = [];
    //
    public static void Add(NetPeer peer, string Name) => Players.Add(peer, new() { Name = Name});
    public static void Remove(NetPeer peer) => Players.Remove(peer);
}