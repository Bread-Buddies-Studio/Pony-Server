using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using LiteNetLib;
using LiteNetLib.Utils;
using Rendering.Entities;

namespace Rendering.Services;

[method: SetsRequiredMembers]
public class ChatEventArgs(NetPeer Client, string Message) : EventArgs
{
    // Data //
    public required NetPeer Client = Client;
    public required string Message = Message;
}

public static class ChatService
{
    // Constructor //
    static ChatService()
    {

    }
    // Data //
    public static List<string> Log
    {
        get => field;

        private set => field = value;
    } = [];
    // Events //
    public static event EventHandler<ChatEventArgs>? Chatted;
    #region Private Methods
    
    #endregion
    // Base Methods //
    public static void AddLog(NetPeer clientSpeaker, string message)
    {
        string speakerName = EntityService.Players[clientSpeaker].Name;
        Log.Add($"{speakerName}: {message}");
        Chatted?.Invoke(speakerName, new ChatEventArgs(clientSpeaker, message));
    }

    
}