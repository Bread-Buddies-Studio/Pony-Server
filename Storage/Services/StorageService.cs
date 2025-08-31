using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Storage.Exceptions;

namespace Storage.Services;

public static class StorageService
{
    // Constants //
    internal static string DataPath = Directory.GetCurrentDirectory() + @"\Data\Storage"; // adjust as needed
    // Data //
    public static string[] FileLines 
    { 
        internal set
        {
            lastUpdate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            foreach (var element in value)
                Console.WriteLine(element);
            File.WriteAllLines(DataPath, value);
            field = value;
        }

        get
        {
            // If Last Fetch was before Last Update
            if (lastFetch > lastUpdate)
            {
                FileLines = File.ReadAllLines(DataPath);
                lastFetch = lastUpdate;
            }

            return field;
        }
    }
    // Timers //
    internal static long lastFetch;
    internal static long lastUpdate;
    // Constructor //
    static StorageService()
    {
        FileLines = File.ReadAllLines(DataPath);
        lastFetch = lastUpdate;
    }
    public static void Store(string data)
    {
        // Condtitions //
        if (Exists(data))
            throw new AlreadyStoredException();
        if (!byte.TryParse(data[..3], out byte tagLine))
            throw new InvalidTaglineExtension();
        // Checks //
        if (FileLines.Length > 0)
        {
            foreach ((int index, var element) in FileLines.Index())
            {
                // Conditions //
                if (data.CompareTo(element) < 0)
                    continue;

                List<string> cache = [.. FileLines];
                cache.Insert(index, data);

                FileLines = [.. cache];

                break;
            }
        }

        else
        {
            FileLines = [data];
        }
    }

    public static bool Exists(string data) => FileLines.Contains(data);
}