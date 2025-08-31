// Imports //
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Directory //
namespace Core.Services;
// Enumerations //
public enum InputAxis
{
    Horizontal,
    Vertical
}
// Service //
public static class InputService
{
    // Arrays //
    public static readonly HashSet<Key> KeysDown = [];
    public static readonly Dictionary<string, Key> Controls = new()
    {
        {"left", Key.Left},
        {"right", Key.Right},
        {"up", Key.Up},
        {"down", Key.Down}
    };
    // Events //
    public static event EventHandler<KeyEventArgs>? KeyDown;
    public static event EventHandler<KeyEventArgs>? KeyUp;
    // Base Methods //
    public static bool IsKeyDown(Key key) => KeysDown.Contains(key);
    public static bool IsKeyDown(string key) => IsKeyDown(Controls[key]);
    public static int GetAxis(InputAxis axisType)
    {
        // Initialize Axis //
        int axis = default;
        // Check Horizontal //
        if (axisType is InputAxis.Horizontal)
        {
            if (IsKeyDown("left"))
                axis--;
            if (IsKeyDown("right"))
                axis++;
        }
        // Check Vertical //
        else
        {
            if (IsKeyDown("up"))
                axis--;
            if (IsKeyDown("down"))
                axis++;
        }
        // Finish //
        return axis;
    }
    // Event Methods //
    public static void OnKeyDown(object? sender, KeyEventArgs eventData)
    {
        KeysDown.Add(eventData.Key);
        KeyDown?.Invoke(sender, eventData);
    }
    public static void OnKeyUp(object? sender, KeyEventArgs eventData)
    {
        KeysDown.Remove(eventData.Key);
        KeyUp?.Invoke(sender, eventData);
    }
}
