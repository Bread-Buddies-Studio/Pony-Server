// Directory //
namespace ServerSpace.Services;
// Public Class //
public static class ChatService
{
    // Internal Data //
    internal static readonly List<string> ChatLog = [];
    // Public Methods //
    public static List<string> GetLog()
    {
        return ChatLog;
    }

    public static void AddMessage(string userName, string message)
    {
        ChatLog.Insert(0, "[b]" + userName + ": [/b]" + message);
    }

    public static void RemoveMessage(string message)
    {
        ChatLog.Remove(message);
    }

    public static void RemoveMessage(int index)
    {
        ChatLog.RemoveAt(index);
    }
}