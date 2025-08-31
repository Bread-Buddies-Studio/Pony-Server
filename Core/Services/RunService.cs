// Imports //
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Directory //
namespace Core.Services;
// Event Args //
public class UpdateEventArgs : EventArgs
{
    // Private Data //
    private static long _lastInvoke = GetCurrentMillis();
    // Public Data //
    public readonly long DeltaTime;
    // Constructor //
    public UpdateEventArgs() : base()
    {
        // Data //
        long currentMillis = GetCurrentMillis();
        // Settings //
        DeltaTime = currentMillis - _lastInvoke;
        // Reset //
        _lastInvoke = GetCurrentMillis();
    }
    // Private Methods //
    static long GetCurrentMillis() => DateTimeOffset.Now.ToUnixTimeMilliseconds();
}
// Class //
public static class RunService
{
    // Public Events //
    public static event EventHandler<UpdateEventArgs>? Update;
    // Private Methods //
    public static void InvokeUpdate() => Update?.Invoke(null, new UpdateEventArgs());
    // Public Methods //
    public static async Task UntilNextUpdate()
    {
        // Event Resetter to wait 1 frame //
        TaskCompletionSource<bool> taskAwaiter = new();
        // Methods //
        void OnUpdate(object? sender, UpdateEventArgs _) => taskAwaiter.SetResult(true);
        // Connections //
        Update += OnUpdate;
        // Await Frame //
        await taskAwaiter.Task;
        // Disconnect //
        Update -= OnUpdate;
    }
}
